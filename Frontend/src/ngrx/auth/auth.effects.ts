import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Actions, createEffect, ofType } from '@ngrx/effects';
import { Store } from '@ngrx/store';
import { CookieService } from 'ngx-cookie-service';
import { of } from 'rxjs';
import { catchError, map, switchMap, tap } from 'rxjs/operators';
import { AuthService } from 'src/business/services/features/auth.service';
import { UIProgressBar } from 'src/business/shared/ui-progress-bars.enum';
import { CurrentUser } from 'src/models/authorization/current-user.model';
import { SignInRequest } from 'src/models/authorization/sign-in.request';
import { UserSetting } from 'src/models/domain-entities/user-settings.model';
import { AppState } from '../global-setup.ngrx';
import { enableErrorDialogs, setActiveProgressBar, switchTheme } from '../user-interface/ui.actions';
import { UserService } from './../../business/services/features/user.service';
import { setLanguage } from './../user-interface/ui.actions';
import * as AuthActions from './auth.actions';

@Injectable()
export class AuthEffects {
  constructor(
    private actions$: Actions,
    private router: Router,
    private cookieService: CookieService,
    private store: Store<AppState>,
    private authService: AuthService,
    private userService: UserService
  ) { }

  login$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.login),
        map(action => action.request),
        switchMap((request: SignInRequest) =>
          this.authService.signIn(request).pipe(
            map((user: CurrentUser) =>
              this.store.dispatch(AuthActions.loginSuccess({ user }))
            ),
            catchError((error: Error) =>
              of(this.store.dispatch(AuthActions.loginFailure({ error })))
            )
          )
        )
      ),
    { dispatch: false }
  );

  loginSuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.loginSuccess),
        map(action => action.user),
        switchMap(
          (currentUser: CurrentUser) => {
            localStorage.setItem('id', currentUser.id);
            return this.router.navigate(['/app']);
          },
          (currentUser, navigationSuccess) => ({ currentUser, navigationSuccess })
        ),
        tap(
          (response: { currentUser: CurrentUser; navigationSuccess: boolean; }) => {
            if (response.navigationSuccess) {
              this.store.dispatch(setLanguage({ language: response.currentUser.userSetting.language }))
              this.store.dispatch(switchTheme({ theme: response.currentUser.userSetting.theme }));
              this.store.dispatch(enableErrorDialogs());
            }
          }
        )
      ),
    { dispatch: false }
  );

  logout$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.logout),
        tap(() => {
          this.authService.signOutEvent.next(true);
          localStorage.removeItem('id');
          this.cookieService.delete('jwt');
          this.router.navigate(['/auth/login']).then(
            _ => {
              this.store.dispatch(AuthActions.logoutClearState())
            }
          );
        })
      ),
    { dispatch: false }
  );

  updateUserSetting$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.updateUserSetting),
        map(action => action.settings),
        tap((userSetting: UserSetting) => {
          this.store.dispatch(switchTheme({ theme: userSetting.theme }));
        }),
        switchMap((userSetting: UserSetting) => this.userService.saveSettings(userSetting), (setting) => setting),
        map((settings) => this.store.dispatch(AuthActions.settingsUpdated({ settings })))
      ),
    { dispatch: false }
  );

  updateCurrentUser$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.updateCurrentUser),
        map(action => action.user),
        tap((currentUser: CurrentUser) => {
          this.store.dispatch(setActiveProgressBar({ progressBar: UIProgressBar.MainAppScreen }));
          this.store.dispatch(setLanguage({ language: currentUser.userSetting.language }))
          this.store.dispatch(switchTheme({ theme: currentUser.userSetting.theme }));
        })
      ),
    { dispatch: false }
  );
}
