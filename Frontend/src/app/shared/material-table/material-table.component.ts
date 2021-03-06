import { animate, state, style, transition, trigger } from '@angular/animations';
import { SelectionModel } from '@angular/cdk/collections';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { AfterViewInit, Component, ElementRef, EventEmitter, Input, OnDestroy, OnInit, Output, Renderer2, ViewChild, ViewEncapsulation } from '@angular/core';
import { MediaObserver } from '@angular/flex-layout';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort, Sort } from '@angular/material/sort';
import { MatTable } from '@angular/material/table';
import _ from "lodash";
import { Observable, Subject } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { TableConfig } from "src/app/shared/material-table/table-models/table-config.model";
import { TableDatasource } from "src/app/shared/material-table/table-models/table-datasource.model";
import { SubSink } from 'subsink';
import { CustomColumn } from "./table-models/custom-column.model";
import { PagingModel } from './table-models/paging.model';

@Component({
  selector: 'app-material-table',
  templateUrl: './material-table.component.html',
  styleUrls: [
    './material-table.component.scss',
  ],
  encapsulation: ViewEncapsulation.None,
  animations: [
    trigger('detailExpand', [
      state('collapsed, void', style({ height: '0px', minHeight: '0', visibility: 'hidden' })),
      state('expanded', style({ height: '*', visibility: 'visible' })),
      transition('expanded <=> collapsed', animate('0ms')),
      transition('expanded <=> void', animate('0ms'))
    ])
  ],
})
export class MaterialTableComponent implements OnInit, AfterViewInit, OnDestroy {

  @Input() datasource: TableDatasource<any>;
  @Input() columns: CustomColumn[];
  @Input() config: TableConfig;

  @Output() selectEvent = new EventEmitter<any>();
  @Output() expandEvent = new EventEmitter<any>();
  @Output() collapseEvent = new EventEmitter<any>();
  @Output() dropEvent = new EventEmitter<any>();

  @Output() addEvent = new EventEmitter<any>();
  @Output() updateEvent = new EventEmitter<any>();
  @Output() deleteEvent = new EventEmitter<any>();
  @Output() disableEvent = new EventEmitter<any>();

  @Output() updateManyEvent = new EventEmitter<any>();
  @Output() disableManyEvent = new EventEmitter<any[]>();
  @Output() deleteManyEvent = new EventEmitter<any[]>();

  @Output() pagingChangeEvent = new EventEmitter<PagingModel>();

  displayColumns: string[] = [];
  selection = new SelectionModel<string>(true, []);
  pageSize: number;
  pageSizeOptions: number[];
  totalItems: Observable<number>;

  @ViewChild(MatTable, { static: true }) table: MatTable<any>;
  @ViewChild(MatTable, { static: true, read: ElementRef }) tableNative: ElementRef;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @ViewChild('filter') filter: ElementRef;
  applyFilterEvent = new Subject<string>();

  private pagingModel: PagingModel;

  expandedRow: any;

  private subs = new SubSink();

  constructor(
    private mediaObserver: MediaObserver,
    private renderer: Renderer2
  ) { }

  ngOnInit() {
    this.pageSize = this.config.pagingOptions.pageSize;
    this.pageSizeOptions = this.config.pagingOptions.pageSizeOptions;

    // assign paginator and sort components to datasource
    // only if we'r not using server side paging
    if (!this.config.pagingOptions.serverSidePaging) {

      // paginator
      this.datasource.paginator = this.paginator;
      // sort
      this.datasource.sort = this.sort

      // do default sort
      setTimeout(() => {
        this.datasource.sortingDataAccessor = this.customSortDataAccessor.bind(this);
        this.datasource.sort.active = this.config.defaultSort;
        this.datasource.sort.direction = this.config.defaultSortDirection;
        this.datasource.sort.sort({
          id: this.config.defaultSort,
          start: this.config.defaultSortDirection,
          disableClear: false
        });
      });
    } else {
      setTimeout(() => this.setTablePagingVariables(this.pagingModel, this.datasource.totalLength()));
    }

    this.subs.add(
      // filter event
      this.onFilterEvent(),
      // do work on viewport change (mobile, tablet, desktop)
      this.onViewportChange(),
      // setup paging
      this.onDatasourcePagingChange()
    );
  }

  ngAfterViewInit() {

  }

  ngOnDestroy() {
    this.subs.unsubscribe();
  }

  onFilterEvent() {
    return this.applyFilterEvent
      .pipe(debounceTime(500))
      .subscribe(filter => this.applyFilter(filter));
  }

  onDatasourcePagingChange() {
    return this.datasource.pagingModel().subscribe(model => {
      this.pagingModel = model;
      this.setTablePagingVariables(model, this.datasource.totalLength());
    })
  }

  onViewportChange() {
    return this.mediaObserver.media$.subscribe(change => {
      var isMobile = change.mqAlias == 'xs';
      this.setupColumns(isMobile);
    })
  }

  setupColumns(isMobile: boolean) {
    const actions = [];

    if (this.config.selectionEnabled)
      actions.push('select');

    if (isMobile) {
      actions.push(...this.columns.filter(x => x.displayOnMobile).map(x => x.definition));
    } else {
      actions.push(...this.columns.map(x => x.definition));
    }

    if (this.config.actionsEnabled)
      actions.push('actions');

    this.displayColumns = actions;
  }

  // sets custom sort by definition from columns config
  // if no custom sortFn is given.. defaults back to item
  customSortDataAccessor(data, prop) {
    let customSortDataFn = this.columns?.filter(x => x.sort && x.sortFn && x.definition == prop)?.map(x => x.sortFn)[0];

    if (customSortDataFn)
      return customSortDataFn(data);

    return data[prop];
  }

  setTablePagingVariables(model: PagingModel, totalItems: Observable<number>) {
    this.totalItems = totalItems;
    this.paginator.pageIndex = model.page;
    this.sort.active = model.sortBy;
    this.sort.direction = model.sortDirection;

    if (this.filter) {
      this.filter.nativeElement.value = model.filterQuery ? model.filterQuery : '';
    }
  }

  applyFilter(filterValue: string) {

    let filterValueFormatted = filterValue.trim().toLocaleLowerCase();

    // TODO - filter this server side - send event for pagination change
    if (this.config.pagingOptions.serverSidePaging) {

      this.pagingModel.filterQuery = filterValueFormatted;
      this.pagingModel.page = 0;
      setTimeout(_ => this.pagingModel.filterQuery = filterValue);
      this.pagingChangeEvent.emit(this.pagingModel);

    } else {

      if (this.config.filterFunction) {
        this.datasource.filterPredicate = this.config.filterFunction;
      }

      this.datasource.filter = filterValueFormatted;

      if (this.datasource.paginator) {
        this.datasource.paginator.firstPage();
      }
    }

  }

  clearSelection() {
    this.selection.clear();
  }

  masterToggle() {
    if (this.isAllSelected) {
      this.selection.clear();
      this.selectEvent.emit(null);
    } else {
      this.datasource.data.forEach(row => this.selection.select(row.id));
    }
  }

  get isOneSelected() {
    return this.selection.selected.length == 1;
  }

  get isMoreThanOneSelected() {
    return this.selection.selected.length > 1;
  }

  get isAllSelected() {
    return this.datasource.data.length === this.selection.selected.length;
  }

  oneSelected = (row) => this.isOneSelected && this.selection.isSelected(row.id)

  onSelect(entity: any, keepSelected: boolean = false) {


    if (!entity) {
      this.selection.clear();
      return this.selectEvent.emit(null);
    }

    if (this.config.usesExpandableRows) {
      this.handleExpandableRow(entity);
    }

    var isSelected = this.selection.isSelected(entity.id);
    this.selection.clear();

    if (isSelected && !keepSelected) { // if it wasn't selected
      return this.selectEvent.emit(null);
    }

    // new selection
    this.selection.toggle(entity.id); // toggle
    this.selectEvent.emit(entity);
  }

  handleExpandableRow(row: any) {
    this.expandedRow = this.expandedRow === row ? null : row;
    if (this.expandedRow) {
      this.expandEvent.emit();
    } else {
      this.collapseEvent.emit();
    }
  }

  onListDrop(event: CdkDragDrop<any[]>) {

    let from = this.paginator.pageIndex * this.paginator.pageSize;
    let to = from + this.paginator.pageSize;

    const array = [...this.datasource.data.slice(from, to)];

    moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);

    // needed for drag and drop to work properly inside table
    this.datasource.data = _.cloneDeep(event.container.data);
    this.table.renderRows();

    this.dropEvent.emit({ previous: array[event.previousIndex], current: array[event.currentIndex] });
  }

  renderRows = (execute: boolean) => execute && this.table.renderRows(); // because on first load we get error.. no data

  get datasourceEmpty(): boolean {
    return !this.datasource.data || this.datasource.data.length == 0;
  }

  get paginatorHidden(): boolean {
    return this.datasourceEmpty || !(this.datasource.data.length > this.pageSize);
  }

  onPageChange(page: PageEvent) {
    this.pagingModel.page = page.pageIndex;
    this.pagingModel.pageSize = page.pageSize;
    this.pagingChangeEvent.emit(this.pagingModel);
    this.onSelect(null);
  }

  onSortChange(sort: Sort) {
    this.pagingModel.sortBy = sort.active;
    this.pagingModel.sortDirection = sort.direction;
    this.pagingChangeEvent.emit(this.pagingModel);
    this.onSelect(null);
  }

  onDragStarted() {
    this.renderer.addClass(this.tableNative.nativeElement, 'pointer-events-none');
  }

  onDragEnded() {
    this.renderer.removeClass(this.tableNative.nativeElement, 'pointer-events-none');
  }
}
