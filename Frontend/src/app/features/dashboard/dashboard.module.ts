import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { SharedModule } from 'src/app/shared/shared.module';
import { ContactsResolver } from 'src/business/resolvers/contacts.resolver';
import { ContactService } from 'src/business/services/features/contacts.service';
import { UIService } from 'src/business/services/shared/ui.service';
import { ContactEffects } from 'src/ngrx/contacts/contact.effects';
import { contactReducer } from 'src/ngrx/contacts/contact.reducers';
import { PhoneValidationService } from './../../../business/services/shared/phone-validation.service';
import { PhoneNumberValidatior } from './../../../business/validators/phone-number.validator';
import { ContactCreateEditComponent } from './dashboard-home/contact-create-edit/contact-create-edit.component';
import { ContactDetailsComponent } from './dashboard-home/contact-details/contact-details.component';
import { ContactListComponent } from './dashboard-home/contact-list/contact-list.component';
import { DashboardHomeComponent } from './dashboard-home/dashboard-home.component';
import { DashboardRoutingModule } from './dashboard-routing.module';


@NgModule({
    imports: [
        SharedModule,
        DashboardRoutingModule,
        StoreModule.forFeature('contact', contactReducer),
        EffectsModule.forFeature([ContactEffects]),
    ],
    declarations: [
        DashboardHomeComponent,
        ContactListComponent,
        ContactDetailsComponent,
        ContactCreateEditComponent
    ],
    exports: [
    ],
    providers: [
        UIService,
        ContactService,
        ContactsResolver,
        PhoneValidationService,
        PhoneNumberValidatior,
    ],
    entryComponents: [
    ]
})
export class DashboardModule { }
