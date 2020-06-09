import { Component, OnDestroy, OnInit } from '@angular/core';
import { MediaObserver } from '@angular/flex-layout';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { UIProgressBar } from 'src/business/shared/ui-progress-bars.enum';
import { UISidenav, UISidenavAction } from 'src/business/shared/ui-sidenavs.enum';
import { AccountType } from 'src/models/enums/account-type.enum';
import { logout } from 'src/ngrx/auth/auth.actions';
import { currentUser } from 'src/ngrx/auth/auth.selectors';
import { AppState } from 'src/ngrx/global-setup.ngrx';
import { getLoadingState } from 'src/ngrx/user-interface/ui.selectors';
import { SubSink } from 'subsink';
import { SettingsComponent } from '../../settings/settings.component';
import { UIService } from './../../../../business/services/shared/ui.service';

@Component({
  selector: 'app-toolbar',
  templateUrl: './toolbar.component.html',
  styleUrls: ['./toolbar.component.scss']
})
export class ToolbarComponent implements OnInit, OnDestroy {

  constructor(
    private store: Store<AppState>,
    private UIService: UIService,
    public mediaObserver: MediaObserver,
    private route: ActivatedRoute
  ) { }

  private _subSink = new SubSink();
  isPublic = false;

  // chat
  // unreadChatMessages: Observable<number>

  loading$: Observable<boolean>;
  dashboardActive$: Observable<boolean>;

  userInfo: {
    userId: string;
    avatar: string;
    fullName: string;
    isCoach: boolean;
    isAthlete: boolean;
  }

  ngOnInit(): void {
    this.isPublic = this.route.snapshot.data?.isPublic;

    // set observable for main progress bar
    this.loading$ = getLoadingState(this.store, UIProgressBar.MainAppScreen);

    if (!this.isPublic) {
      this._subSink.add(
        this.store.select(currentUser)
          .subscribe(user => {
            this.userInfo = {
              userId: user.id,
              isCoach: user.accountType == AccountType.Coach || user.accountType == AccountType.Admin,
              isAthlete: user.accountType == AccountType.Athlete,
              fullName: user.fullName,
              avatar: user?.avatar,
            }
          }),
      )
    }

    // this.unreadChatMessages = this.store.select(totalUnreadChatMessages);
  }

  ngOnDestroy(): void {
    this._subSink.unsubscribe();
  }

  onOpenSettings(section: string = null) {
    let dialogRef = this.UIService.openDialogFromComponent(SettingsComponent, {
      height: 'auto',
      width: '98%',
      maxWidth: '58rem',
      autoFocus: false,
      data: { title: 'SETTINGS.SETTINGS_LABEL', section },
      panelClass: ['settings-dialog-container'],
      closeOnNavigation: true
    });

    dialogRef.afterClosed().pipe(take(1)).subscribe(_ => console.log(_));
  }

  onLogout() {
    this.store.dispatch(logout());
  }

  onToggleSidebar() {
    this.UIService.doSidenavAction(UISidenav.App, UISidenavAction.Toggle);
  }

}
