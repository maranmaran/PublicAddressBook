import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { PagingModel } from 'src/app/shared/material-table/table-models/paging.model';
import { Contact } from 'src/models/domain-entities/contact.model';
import { PagedList } from 'src/models/shared/paged-list.model';
import { CrudService } from '../crud.service';

@Injectable({ providedIn: 'root' })
export class ContactService extends CrudService<Contact> {

    constructor(
        private httpDI: HttpClient,
    ) {
        super(httpDI, "Contacts");
    }

    public getPaged(paginationModel: PagingModel) {

        var request = {
            paginationModel
        }

        return this.http.post<PagedList<Contact>>(this.url + 'GetAll/', request)
            .pipe(
                catchError(this.handleError)
            );

    }
}
