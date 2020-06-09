import { Component, Input, OnInit } from '@angular/core';
import { Notification } from 'src/models/domain-entities/notification.model';
import { NotificationType } from 'src/models/enums/notification-type.enum';

@Component({
  selector: 'app-notification-item',
  templateUrl: './notification-item.component.html',
  styleUrls: ['./notification-item.component.scss']
})
export class NotificationItemComponent implements OnInit {


  @Input() model: Notification;
  notificationType = NotificationType;


  constructor() { }

  ngOnInit(): void {
  }

}
