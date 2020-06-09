import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { AuthRoutingModule } from './auth-routing.module';
import { LoginComponent } from './login/login.component';

@NgModule({
    imports: [
        AuthRoutingModule,
        SharedModule,
    ],
    declarations: [
        LoginComponent,
    ],
    exports: [
    ],
    providers: [
    ],
    entryComponents: [
    ]
})
export class AuthModule { }
