import { Component, HostListener, OnInit, ViewChild } from '@angular/core';
import { MediaObserver } from '@angular/flex-layout';
import { MatSidenav } from '@angular/material/sidenav';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { NotificationSignalrService } from 'src/business/services/features/notification-signalr.service';
import { UIService } from 'src/business/services/shared/ui.service';
import { UISidenav, UISidenavAction } from 'src/business/shared/ui-sidenavs.enum';
import { AppState } from 'src/ngrx/global-setup.ngrx';
import { SubSink } from 'subsink';
import { ToolbarComponent } from '../navigation/toolbar/toolbar.component';

@Component({
  selector: 'app-app-container',
  templateUrl: './app-container.component.html',
  styleUrls: ['./app-container.component.scss']
})
export class AppContainerComponent implements OnInit {

  @ViewChild(MatSidenav, { static: true }) sidenav: MatSidenav;
  @ViewChild(ToolbarComponent, { static: true }) toolbar: ToolbarComponent;

  private _subs = new SubSink();

  constructor(
    public mediaObserver: MediaObserver,
    public store: Store<AppState>,
    private route: ActivatedRoute,
    private uiService: UIService,
    private notificationService: NotificationSignalrService, // just here to be instantiated
  ) { }

  ngOnInit(): void {
    // set sidenav
    this.uiService.addOrUpdateSidenav(UISidenav.App, this.sidenav);

    let settingsSection = this.route.snapshot.data.section;
    if (settingsSection) {
      this.toolbar.onOpenSettings(settingsSection);
    }
  }

  ngOnDestroy() {
    this._subs.unsubscribe();
  }

  //#region CHAT 
  // // chat variables
  // theme: ChatTheme;
  // fullScreenChatActive: boolean;
  // chatConfig: ChatConfiguration;

  // onChatInit() {
  //   // chat theme subscription
  //   return [
  //     this.store.select(activeTheme)
  //         .subscribe((theme: Theme) => {
  //           this.theme = ChatTheme[theme]
  //           // this.chatConfig.theme = ChatTheme[theme];
  //         }),
  //       this.store.select(isFullScreenChatActive)
  //         .subscribe(isFullScreenChatActive => this.fullScreenChatActive = isFullScreenChatActive),
  //   ]
  // }

  // get showSmallChat(): boolean {
  //   return !this.fullScreenChatActive && !this.mediaObserver.isActive('lt-md')
  // }
  //#endregion

  @HostListener('swiperight', ['$event']) onSwipeRight = () => {
    if (!this.uiService.isSidenavOpened(UISidenav.App, true))
      this.uiService.doSidenavAction(UISidenav.App, UISidenavAction.Toggle)
  }

  @HostListener('swipeleft', ['$event']) onSwipeLeft = () => {
    if (this.uiService.isSidenavOpened(UISidenav.App, true))
      this.uiService.doSidenavAction(UISidenav.App, UISidenavAction.Toggle)
  }


}
