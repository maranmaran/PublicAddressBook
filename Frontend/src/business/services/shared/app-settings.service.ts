import { ComponentType } from '@angular/cdk/portal';
import { HttpHeaders } from '@angular/common/http';
import { Inject, Injectable, InjectionToken } from '@angular/core';
import { environment } from 'src/environments/environment';

export const NOTIFICATION_TOAST_TOKEN: InjectionToken<ComponentType<any>> =
  new InjectionToken<ComponentType<any>>('NOTIFICATION_TOAST_TOKEN');

@Injectable({ providedIn: 'root' })
export class AppSettingsService {

  constructor(
    @Inject(NOTIFICATION_TOAST_TOKEN) private toastNotificationComponent: ComponentType<any>
  ) { }

  /**
   * Returns the base URL for the API.
   */
  public get apiUrl(): string {
    // return environment.apiUrl;
    return environment.apiUrlNoSSL;
  }

  /**
 * Returns the base URL for the Push notifications hub.
 */
  public get notificationHubUrl(): string {
    return environment.notificationHubUrlNoSSL;
  }

  /**
   * Returns http headers for the API.
   */
  public get apiHeaders(): HttpHeaders {
    const headers: HttpHeaders = new HttpHeaders({ 'Content-Type': 'application/json' });
    return headers;
  }

  /**
   * Returns the current environment name.
   */
  public get environmentName(): string {
    return environment.name;
  }

  /**
   * Flags whether stack traces will be shown when displaying errors.
   */
  public get showStackTrace(): boolean {
    return environment.showStackTrace;
  }

  /*
  * Api key for phone validation service.. 
  */
  public get phoneValidationServiceAPIKey(): string {

    if (!environment.phoneValidationServiceAPIKey || environment.phoneValidationServiceAPIKey == 'API_KEY')
      throw new Error("Please add API key");

    return environment.phoneValidationServiceAPIKey;
  }

  // public get addressValidationServiceAPIKey(): string {

  //   if (!environment.addressValidationServiceAPIKey || environment.addressValidationServiceAPIKey == 'API_KEY')
  //     throw new Error("Please add API key");

  //   return environment.addressValidationServiceAPIKey;
  // }

  // public get addressValidationServiceUserID(): string {

  //   if (!environment.addressValidationServiceUserID || environment.addressValidationServiceUserID == 'USER_ID')
  //     throw new Error("Please add User ID");

  //   return environment.addressValidationServiceUserID;
  // }

  // Custom toast
  public get systemNotificationToastConfig() {
    return {
      timeOut: 2000,
      disableTimeOut: false,
      positionClass: 'toast-bottom-right',
      preventDuplicates: false,
      toastClass: 'toast-notification',
      toastComponent: this.toastNotificationComponent // added custom toast!
    };
  }

  // Default toast
  public get defaultNotificationConfig() {
    return {
      timeOut: 2000,
      disableTimeOut: false,
      positionClass: 'toast-bottom-right',
      preventDuplicates: false,
      enableHtml: true,
      toastClass: 'toast-default'
    }
  }

}

export const APP_SETTINGS_PROVIDER = [
  { provide: AppSettingsService, useClass: AppSettingsService }
];
