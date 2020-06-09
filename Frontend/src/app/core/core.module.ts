import { CommonModule } from '@angular/common';
import { HttpBackend, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { forwardRef, NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatPaginatorIntl } from '@angular/material/paginator';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule } from '@angular/router';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { StoreDevtoolsModule } from '@ngrx/store-devtools';
import { TranslateLoader, TranslateModule, TranslateService } from '@ngx-translate/core';
import { CookieService } from 'ngx-cookie-service';
import { ToastrModule } from 'ngx-toastr';
import { PaginatorTranslateFactory } from 'src/assets/i18n/paginator-translate.factory';
import { HttpLoaderFactory } from 'src/assets/i18n/translation-http-loader.factory';
import { CurrentUserLoadedGuard } from 'src/business/guards/current-user-loaded.guard';
import { ErrorInterceptor } from 'src/business/interceptors/error.interceptor';
import { HttpInterceptor } from 'src/business/interceptors/http.interceptor';
import { APP_SETTINGS_PROVIDER, NOTIFICATION_TOAST_TOKEN } from 'src/business/services/shared/app-settings.service';
import { UIService } from 'src/business/services/shared/ui.service';
import { environment } from 'src/environments/environment';
import { metaReducers, reducers } from 'src/ngrx/global-setup.ngrx';
import { MaterialModule } from '../shared/angular-material.module';
import { SharedModule } from '../shared/shared.module';
import { SignalrHubsModule } from '../shared/signalr-hubs.module';
import { CoreEffects } from './../../ngrx/global-setup.ngrx';
import { NotificationToastComponent } from './../shared/notifications/notification-toast/notification-toast.component';
import { AppContainerComponent } from './app-container/app-container.component';
import { CoreRoutingModule } from './core-routing.module';
import { SidebarComponent } from './navigation/sidebar/sidebar.component';
import { NotificationsComponent } from './navigation/toolbar/notifications/notifications.component';
import { ToolbarComponent } from './navigation/toolbar/toolbar.component';
import { AccountComponent } from './settings/account/account.component';
import { GeneralComponent } from './settings/general/general.component';
import { SettingsComponent } from './settings/settings.component';

@NgModule({
    imports: [
        SharedModule,
        CommonModule,
        BrowserAnimationsModule,
        CoreRoutingModule,
        HttpClientModule,
        ReactiveFormsModule,
        MaterialModule,
        // NgxStripeModule.forRoot(environment.stripePublishableKey),
        StoreModule.forRoot(reducers, {
            metaReducers,
            runtimeChecks: {
                strictStateImmutability: true,
                strictActionImmutability: true,
            }
        }),
        StoreDevtoolsModule.instrument({ maxAge: 25, logOnly: environment.production }),
        EffectsModule.forRoot(CoreEffects),
        ToastrModule.forRoot(), // ToastrModule added,
        SignalrHubsModule.forRoot(),
        TranslateModule.forRoot({
            isolate: false,
            loader: {
                provide: TranslateLoader,
                useFactory: HttpLoaderFactory,
                deps: [HttpBackend]
            }
        })
    ],
    declarations: [
        AppContainerComponent,
        SidebarComponent,
        ToolbarComponent,
        SettingsComponent,
        NotificationsComponent,
        GeneralComponent,
        AccountComponent,
        NotificationToastComponent
    ],
    exports: [
        CommonModule,
        ReactiveFormsModule,
        MaterialModule,
        RouterModule,
    ],
    providers: [
        CurrentUserLoadedGuard,
        UIService,
        CookieService,
        APP_SETTINGS_PROVIDER,
        { provide: HTTP_INTERCEPTORS, useClass: HttpInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
        {
            provide: MatPaginatorIntl, deps: [TranslateService],
            useFactory: (translateService: TranslateService) => new PaginatorTranslateFactory(translateService).getPaginatorIntl()
        },
        { provide: NOTIFICATION_TOAST_TOKEN, useFactory: forwardRef(() => NotificationToastComponent) }
        // { provide: MAT_HAMMER_OPTIONS, useClass: CustomHammerJsConfig }
        // { provide: APP_INITIALIZER, useFactory: initApplication, multi: true, deps: [Store, Actions] }
    ],
    entryComponents: [
        NotificationToastComponent
    ]
})
export class CoreModule { }
