import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot } from '@angular/router';
import { Store } from '@ngrx/store';
import { forkJoin, Observable } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { isPayingUser, isSubscribed, isTrialing, trialDaysRemaining } from 'src/ngrx/auth/auth.selectors';
import { AppState } from 'src/ngrx/global-setup.ngrx';
import { UIService } from '../services/shared/ui.service';

@Injectable({ providedIn: 'root' })
export class SubscriptionGuard implements CanActivate {

  constructor(
    private UIService: UIService,
    private store: Store<AppState>
  ) {
  }


  canActivate(next: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {

    // In the next you have .url prop so you can check for it 
    const allowedToNavigate = next.url ? next.url.length > 0 ? next.url[0].path == 'settings' || next.url[0].path == 'billing' : false : false;

    return forkJoin(

      this.store.select(isPayingUser).pipe(take(1)),
      this.store.select(isTrialing).pipe(take(1)),
      this.store.select(isSubscribed).pipe(take(1)),
      this.store.select(trialDaysRemaining).pipe(take(1)))

      .pipe(map(([isPayingUser, isTrialing, isSubscribed, trialDaysRemaining]) => {

        if (isPayingUser) {
          !allowedToNavigate && this.UIService.showSubscriptioninfoDialogOnLogin(isTrialing, isSubscribed, trialDaysRemaining);
        }

        // if (isTrialing || isSubscribed || allowedToNavigate) return true;

        return true;

      }));
  }

}
