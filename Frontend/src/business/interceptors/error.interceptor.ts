import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Store } from '@ngrx/store';
import { throwError } from 'rxjs';
import { Observable } from 'rxjs/internal/Observable';
import { catchError } from 'rxjs/operators';
import { ErrorDetails } from 'src/models/error/error-details.model';
import { AppState } from 'src/ngrx/global-setup.ngrx';
import { httpErrorOccured } from 'src/ngrx/user-interface/ui.actions';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

    constructor(
        private store: Store<AppState>
    ) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        return next.handle(req)
            .pipe(
                catchError((error: HttpErrorResponse) => {

                    const serverError = error.error as ErrorDetails;

                    //Ignore Unauthorized because we will be redirected to login
                    if (error?.status != 401) {
                        this.store.dispatch(httpErrorOccured(serverError?.message))
                    }

                    return throwError(error);
                }),
            );;
    }
}
