import { CommonModule } from '@angular/common';
import { HttpBackend } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { DynamicModule } from 'ng-dynamic-component';
import { ImageCropperModule } from 'ngx-image-cropper';
import { ConfirmDialogComponent } from 'src/app/shared/dialogs/confirm-dialog/confirm-dialog.component';
import { MessageDialogComponent } from 'src/app/shared/dialogs/message-dialog/message-dialog.component';
import { ImageCropperComponent } from 'src/app/shared/media/image-cropper/image-cropper.component';
import { HttpLoaderFactory } from 'src/assets/i18n/translation-http-loader.factory';
import { ButtonSizeDirective } from 'src/business/directives/button-size.directive';
import { ShowHidePasswordDirective } from 'src/business/directives/show-hide-password.directive';
import { RepeatPipe } from 'src/business/pipes/repeat.pipe';
import { EnumToArrayPipe } from './../../business/pipes/enum-to-array.pipe';
import { SanitizeHtmlPipe } from './../../business/pipes/sanitize-html.pipe';
import { SplitPascalCasePipe } from './../../business/pipes/split-pascal-case.pipe';
import { MaterialModule } from './angular-material.module';
import { MediaDialogComponent } from './dialogs/media-dialog/media-dialog.component';
import { MaterialTableComponent } from './material-table/material-table.component';
import { NotImplementedComponent } from './not-implemented/not-implemented.component';
import { ContactBodyTemplateComponent } from './notifications/notification-item/body-templates/contact-body-template.component';
import { NotificationItemComponent } from './notifications/notification-item/notification-item.component';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        RouterModule,
        MaterialModule,
        ImageCropperModule,
        DynamicModule.withComponents([
        ]),
        // lazy loaded modules import of translate module
        // https://github.com/ngx-translate/core#1-import-the-translatemodule
        TranslateModule.forChild({
            isolate: false,
            extend: true,
            loader: {
                provide: TranslateLoader,
                useFactory: HttpLoaderFactory,
                deps: [HttpBackend]
            }
        }),
    ],
    declarations: [
        SanitizeHtmlPipe,
        EnumToArrayPipe,
        SplitPascalCasePipe,
        ImageCropperComponent,
        // ApplyTimezonePipe,
        ButtonSizeDirective,
        RepeatPipe,
        MediaDialogComponent,
        ConfirmDialogComponent,
        MessageDialogComponent,
        ShowHidePasswordDirective,
        NotImplementedComponent,
        MaterialTableComponent,

        NotificationItemComponent,
        ContactBodyTemplateComponent
    ],
    exports: [
        CommonModule,
        ReactiveFormsModule,
        RouterModule,
        MaterialModule,
        ImageCropperModule,
        ImageCropperComponent,
        // export translate module to
        // others who use it
        // https://github.com/ngx-translate/core#1-import-the-translatemodule
        TranslateModule,
        SanitizeHtmlPipe,
        EnumToArrayPipe,
        SplitPascalCasePipe,
        // ApplyTimezonePipe,
        ButtonSizeDirective,
        RepeatPipe,
        MediaDialogComponent,
        ConfirmDialogComponent,
        MessageDialogComponent,
        ShowHidePasswordDirective,
        NotImplementedComponent,
        MaterialTableComponent,

        NotificationItemComponent,
        ContactBodyTemplateComponent
    ],
    providers: [
    ],
    entryComponents: [
    ]
})
export class SharedModule { }
