import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { TranslateService } from '@ngx-translate/core';
import { noop } from 'rxjs';
import { filter, map, switchMap, take } from 'rxjs/operators';
import { MaterialTableComponent } from 'src/app/shared/material-table/material-table.component';
import { CustomColumn } from 'src/app/shared/material-table/table-models/custom-column.model';
import { PagingModel } from 'src/app/shared/material-table/table-models/paging.model';
import { TableAction, TableConfig, TablePagingOptions } from 'src/app/shared/material-table/table-models/table-config.model';
import { TableDatasource } from 'src/app/shared/material-table/table-models/table-datasource.model';
import { ContactService } from 'src/business/services/features/contacts.service';
import { NotificationSignalrService } from 'src/business/services/features/notification-signalr.service';
import { UIService } from 'src/business/services/shared/ui.service';
import { ConfirmDialogConfig, ConfirmResult } from 'src/business/shared/confirm-dialog.config';
import { CRUD } from 'src/business/shared/crud.enum';
import { Contact } from 'src/models/domain-entities/contact.model';
import { NotificationType } from 'src/models/enums/notification-type.enum';
import { PagedList } from 'src/models/shared/paged-list.model';
import { contactDeleted, contactsFetched, setSelectedContact } from 'src/ngrx/contacts/contact.actions';
import { contacts } from 'src/ngrx/contacts/contact.selectors';
import { AppState } from 'src/ngrx/global-setup.ngrx';
import { SubSink } from 'subsink';
import { ContactCreateEditComponent } from '../contact-create-edit/contact-create-edit.component';

@Component({
  selector: 'app-contact-list',
  templateUrl: './contact-list.component.html',
  styleUrls: ['./contact-list.component.scss']
})
export class ContactListComponent implements OnInit {


  private subs = new SubSink();

  tableConfig: TableConfig;
  tableColumns: CustomColumn[];
  tableDatasource: TableDatasource<Contact>;
  @ViewChild(MaterialTableComponent, { static: true }) table: MaterialTableComponent;

  contactControl = new FormControl();
  contacts: Contact[] = [];
  private _isPublic: boolean = false;

  constructor(
    private uiService: UIService,
    private contactService: ContactService,
    private store: Store<AppState>,
    private translateService: TranslateService,
    private route: ActivatedRoute,
    private notificationService: NotificationSignalrService
  ) { }

  ngOnInit() {
    this._isPublic = this.route.snapshot.data.isPublic;

    this.tableDatasource = new TableDatasource([]);
    this.tableConfig = this.getTableConfig();
    this.tableColumns = this.getTableColumns() as CustomColumn[];

    this.subs.add(
      this.onGetDatasource(),
    )

    if (this._isPublic) {
      this.subs.add(this.onNotificationReceived())
    }
  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onNotificationReceived() {
    return this.notificationService.notifications$
      .pipe(
        filter(item => item.type == NotificationType.ContactChanged),
        switchMap(_ => this._fetchContacts(this.pagingModelCache ?? new PagingModel()))
      ).subscribe(noop)
  }

  onGetDatasource() {
    return this.store.select(contacts)
      .subscribe((state: { entities: Contact[], totalItems: number, pagingModel: PagingModel }) => {

        this.contacts = state.entities.slice(0, state.pagingModel.pageSize);

        this.tableDatasource.updateDatasource([...this.contacts]);
        this.tableDatasource.setPagingModel(Object.assign({}, state.pagingModel));
        this.tableDatasource.setTotalLength(state.totalItems);
      });
  }

  getTableConfig() {

    const tableConfig = new TableConfig({
      pagingOptions: new TablePagingOptions({
        serverSidePaging: true
      }),
      headerActions: !this._isPublic ? [TableAction.create] : [],
      cellActions: !this._isPublic ? [TableAction.update, TableAction.delete] : [],
      selectionEnabled: false,
      filterEnabled: true
    });

    return tableConfig;
  }

  getTableColumns() {
    return [
      new CustomColumn({
        definition: 'name',
        title: 'SHARED.NAME',
        sort: true,
        useComponent: false,
        displayFn: (item: Contact) => item.name
      }),
      new CustomColumn({
        definition: 'address',
        title: 'CONTACT.ADDRESS',
        headerClass: 'address-header',
        cellClass: 'address-cell',
        sort: true,
        useComponent: false,
        displayFn: (item: Contact) => item.address
      }),
    ]
  }

  onSelect = (contact: Contact) => this.store.dispatch(setSelectedContact({ entity: contact }));

  openAddDialog() {
    const dialogRef = this.uiService.openDialogFromComponent(ContactCreateEditComponent, {
      height: 'auto',
      width: '98%',
      maxWidth: '30rem',
      autoFocus: false,
      data: { title: 'CONTACT.ADD_CONTACT_TITLE', action: CRUD.Create },
      panelClass: ['contact-dialog-container']
    })

    this.onDialogClose(dialogRef);
  }

  openUpdateDialog(contact: Contact) {
    const dialogRef = this.uiService.openDialogFromComponent(ContactCreateEditComponent, {
      height: 'auto',
      width: '98%',
      maxWidth: '30rem',
      // maxHeight: '50vh',
      autoFocus: false,
      data: { title: this.translateService.instant('CONTACT.UPDATE_CONTACT_TITLE', { name: contact.name }), action: CRUD.Update, contact },
      panelClass: ['contact-dialog-container']
    })

    this.onDialogClose(dialogRef);
  }

  onDialogClose(dialogRef: MatDialogRef<any, any>) {
    dialogRef.afterClosed().pipe(take(1))
      .subscribe((type: Contact) => {
        if (type) {
          this.table.onSelect(type, true);
          this.onSelect(type);
        }
      })
  }

  onDeleteSingle(contact: Contact) {

    var deleteDialogConfig = new ConfirmDialogConfig({ title: 'CONTACT.DELETE_TITLE', confirmLabel: 'SHARED.DELETE' });

    deleteDialogConfig.message = this.translateService.instant('CONTACT.DELETE_BODY', { name: contact.name });

    var dialogRef = this.uiService.openConfirmDialog(deleteDialogConfig);

    dialogRef.afterClosed().pipe(take(1))
      .subscribe((result: ConfirmResult) => {
        if (result == ConfirmResult.Confirm) {
          this.contactService.delete(contact.id)
            .subscribe(
              () => {
                this.store.dispatch(contactDeleted({ id: contact.id }))
              },
              err => console.log(err)
            )
        }
      })
  }

  pagingModelCache: PagingModel;
  onPagingChange(model: PagingModel) {
    this.pagingModelCache = model;
    this._fetchContacts(model).subscribe(_ => _, err => console.log(err));
  }

  _fetchContacts = (model: PagingModel) => {
    return this.contactService.getPaged(model)
      .pipe(
        take(1),
        map(((pagedListModel: PagedList<Contact>) => {
          this.store.dispatch(contactsFetched({ entities: pagedListModel.list, totalItems: pagedListModel.totalItems, pagingModel: model }));
        }))
      );
  }

}
