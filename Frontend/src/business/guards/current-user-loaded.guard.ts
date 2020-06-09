import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable, of } from 'rxjs';
import { catchError, map, take } from 'rxjs/operators';
import { CurrentUser } from 'src/models/authorization/current-user.model';
import { updateCurrentUser } from 'src/ngrx/auth/auth.actions';
import { AppState } from 'src/ngrx/global-setup.ngrx';
import { setActiveProgressBar } from 'src/ngrx/user-interface/ui.actions';
import { AuthService } from '../services/features/auth.service';
import { UIProgressBar } from '../shared/ui-progress-bars.enum';

@Injectable()
export class CurrentUserLoadedGuard implements CanActivate {


    constructor(
        private store: Store<AppState>,
        private authService: AuthService,
        private router: Router
    ) {
    }

    // tslint:disable-next-line: max-line-length
    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {

        this.store.dispatch(setActiveProgressBar({ progressBar: UIProgressBar.SplashScreen }));

        return new Promise(resolve => {
            this.authService.getCurrentUserInfo()
                .pipe(
                    take(1),
                    catchError((err) => {
                        console.log(err);
                        this.router.navigate(['/auth/login']);
                        return of(null);
                    }),
                    map(((user: CurrentUser) => {
                        if (!user) { return resolve(false); }

                        // tslint:disable-next-line: no-unused-expression
                        user && this.store.dispatch(updateCurrentUser({ user }));

                        resolve(true);
                    }))
                ).subscribe();
        });
    }

}
