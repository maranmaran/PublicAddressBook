import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ContactsResolver } from './../../../business/resolvers/contacts.resolver';
import { DashboardHomeComponent } from './dashboard-home/dashboard-home.component';

const routes: Routes = [
    {
        path: '', component: DashboardHomeComponent, resolve: { ContactsResolver }, children: [
        ]
    },
    { path: '**', redirectTo: '/' }, //always last
];

@NgModule({
    imports: [
        RouterModule.forChild(routes)
    ],
    exports: [
        RouterModule
    ],
    providers: [
    ]
})
export class DashboardRoutingModule {
}
