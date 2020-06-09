import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable, of } from 'rxjs';
import { concatMap, map, take } from 'rxjs/operators';
import { PagingModel } from 'src/app/shared/material-table/table-models/paging.model';
import { Contact } from 'src/models/domain-entities/contact.model';
import { PagedList } from 'src/models/shared/paged-list.model';
import { contactsFetched } from 'src/ngrx/contacts/contact.actions';
import { contacts } from 'src/ngrx/contacts/contact.selectors';
import { AppState } from 'src/ngrx/global-setup.ngrx';
import { ContactService } from '../services/features/contacts.service';
import { isEmpty } from '../utils/utils';

@Injectable()
export class ContactsResolver implements Resolve<Observable<Contact[] | void>> {

    constructor(
        private contactService: ContactService,
        private store: Store<AppState>
    ) { }


    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        return this.getState();
    }

    private getState() {

        return this.store
            .select(contacts)
            .pipe(
                take(1),
                map(state => state.entities),
                concatMap((contacts) => {

                    if (isEmpty(contacts)) {
                        return this.updateState();
                    }

                    return of(contacts);
                }));
    }

    private updateState() {

        let defaultPagingModel = new PagingModel();
        defaultPagingModel.sortBy = 'name';
        defaultPagingModel.sortDirection = 'asc';

        return this.contactService.getPaged(defaultPagingModel)
            .pipe(
                take(1),
                map(((pagedListModel: PagedList<Contact>) => {
                    this.store.dispatch(contactsFetched({ entities: pagedListModel.list, totalItems: pagedListModel.totalItems, pagingModel: defaultPagingModel }));
                }))
            );
    }
}
