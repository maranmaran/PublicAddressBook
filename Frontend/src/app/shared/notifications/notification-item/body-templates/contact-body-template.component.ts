import { Component, Input, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Notification } from 'src/models/domain-entities/notification.model';

@Component({
  selector: 'app-contact-body-template',
  template: `<span *ngIf="showSender" [innerHtml]="message"></span>`,
})
export class ContactBodyTemplateComponent implements OnInit {

  @Input() notification: Notification;

  message: string;
  showSender: boolean;

  constructor(
    private translateService: TranslateService
  ) { }

  ngOnInit(): void {
    this.showSender = true;

    const params = {
      name: this.notification.entity.name,
      address: this.notification.entity.address
    }

    this.message = this.translateService.instant('CONTACT.CONTACT_CHANGED_BODY', params);
  }

}
