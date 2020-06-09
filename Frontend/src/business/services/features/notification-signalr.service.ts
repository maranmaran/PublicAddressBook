import { HttpClient } from '@angular/common/http';
import { Injectable, OnDestroy } from '@angular/core';
import * as signalR from '@microsoft/signalr';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { Observable, Subject, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Notification } from 'src/models/domain-entities/notification.model';
import { NotificationType } from 'src/models/enums/notification-type.enum';
import { SubSink } from 'subsink';
import { AppSettingsService } from './../shared/app-settings.service';
import { AuthService } from './auth.service';

@Injectable()
export class NotificationSignalrService implements OnDestroy {

  public notifications$ = new Subject<Notification>();

  hubConnection: signalR.HubConnection;
  subs = new SubSink();

  constructor(
    private authService: AuthService,
    private translate: TranslateService,
    private http: HttpClient,
    private appSettingsService: AppSettingsService,
    public toastService: ToastrService,
  ) {
    // subscribe to logout
    this.subs.add(
      this.authService.signOutEvent.subscribe(() => this.stopConnection())
    );

    // configure connection
    this.configureHubConnection();
  }

  configureHubConnection() {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl(this.appSettingsService.notificationHubUrl)
      // { accessTokenFactory: () => this.authService.getToken() })
      .build();

    this.hubConnection.serverTimeoutInMilliseconds = 100000; // 100 sec
    this.hubConnection.start()
      .then(() => this.initializeListeners())
      .catch(err => console.log(`Error while starting SignalR connection: ${err}`));
  }

  initializeListeners() {
    this.hubConnection.on('SendNotification', (notification: Notification) => {
      notification = this.doWork(notification);
      this.notifications$.next(notification);

      this.sendToast(notification);
    });
  }

  sendToast(notification: Notification) {
    this.toastService.show(JSON.stringify(notification), this.translate.instant('SHARED.NOTIFICATION'), this.appSettingsService.systemNotificationToastConfig)
  }

  // do some specific work based on the received notification type
  doWork(notification: Notification) {
    notification.entity = JSON.parse(notification.jsonEntity);
    // todo additional work
    // switch(notification.type) {
    // }
    return notification;
  }

  getHistory(userId: string, page: number, pageSize: number = 10): Observable<Notification[]> {
    // This could be an API call to your web application that would go to the database
    // and retrieve a N amount of history messages between the users.
    return this.http
      .get<Notification[]>('Notifications/GetNotificationHistory/' + userId + '/' + page + '/' + pageSize)
      .pipe(
        catchError((error: any) => throwError(error.error || 'Server error')),
        map((notifications: Notification[]) => {
          notifications.forEach(notification => {
            notification.entity = JSON.parse(notification.jsonEntity);
            return notification;
          });

          return notifications;
        })
      );
  }

  readNotification(id: string) {
    if (this.hubConnection && this.hubConnection.state === signalR.HubConnectionState.Connected) {
      this.hubConnection.send('ReadNotification', id);
    }
  }

  sendNotification(type: NotificationType, payload: string, senderId: string, receiverId: string): void {
    if (this.hubConnection && this.hubConnection.state === signalR.HubConnectionState.Connected) {
      this.hubConnection.send('SendNotification', type, payload, senderId, receiverId);
    }
  }

  stopConnection() {
    console.log('Stopping notification hub connection');
    this.hubConnection.stop();
  }

  ngOnDestroy(): void {
    this.hubConnection.stop();
  }
}
