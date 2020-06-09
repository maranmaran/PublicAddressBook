import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { MediaObserver } from '@angular/flex-layout';
import { Store } from '@ngrx/store';
import { take } from 'rxjs/internal/operators/take';
import { NotificationSignalrService } from 'src/business/services/features/notification-signalr.service';
import { Notification } from 'src/models/domain-entities/notification.model';
import { AppState } from 'src/ngrx/global-setup.ngrx';
import { SubSink } from 'subsink';

@Component({
  selector: 'app-notifications',
  templateUrl: './notifications.component.html',
  styleUrls: ['./notifications.component.scss']
})
export class NotificationsComponent implements OnInit, OnDestroy {

  // notifications
  page = 0;
  pageSize = 10; // TODO: AppSettings -> NotificationsPageSize
  unreadNotificationCounter = 0;
  notifications: Notification[] = [];
  stopFetch = false;

  @Input() userInfo: {
    userId: string;
    avatar: string;
    fullName: string;
    isCoach: boolean;
    isAthlete: boolean;
  }

  private _subSink = new SubSink();

  constructor(
    private notificationService: NotificationSignalrService,
    private store: Store<AppState>,
    public mediaObserver: MediaObserver
  ) { }

  ngOnInit(): void {
    // subscribe to notifications
    // only new ones.. in real time
    // this.notifications$ = this.notificationService.notifications$;
    this._subSink.add(

      this.notificationService.notifications$
        .subscribe((notification: Notification) => {

          this.notifications = [notification, ...this.notifications];
          !notification.read && this.unreadNotificationCounter++;

        }),
    );

    setTimeout(_ => this.getHistory(this.userInfo.userId, this.page++, this.pageSize));
  }

  ngOnDestroy(): void {
    this._subSink.unsubscribe();
  }

  // BUG with mat menu integration https://github.com/angular/components/issues/10122
  // loadMoreNotifications(index: number) {
  //   console.log(index);
  //   index += 7;

  //   if (index === this.notifications.length && !this.stopFetch) {
  //     this.getHistory(this.page++, this.pageSize);
  //   }

  loadMoreNotifications() {
    this.getHistory(this.userInfo.userId, this.page++, this.pageSize);
  }

  getHistory(userId, page, pageSize) {

    if (!this.stopFetch) {
      this.notificationService.getHistory(userId, page, pageSize)
        .pipe(take(1))
        .subscribe(notifications => {

          if (!notifications || notifications.length == 0) {
            return this.stopFetch = true;
          }

          notifications.forEach(n => !n.read && this.unreadNotificationCounter++);
          this.notifications = [...this.notifications, ...notifications];

        });
    }
  }

  // TODO: Route probably
  onClickNotification(notification: Notification) {
    console.log('click');
    return;
  }

  // TODO: Update notification
  onHoverNotification(notification: Notification) {
    notification.read = true;
    this.unreadNotificationCounter--;
    this.notificationService.readNotification(notification.id);
    return;
  }


}
