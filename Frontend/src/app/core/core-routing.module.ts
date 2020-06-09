import { NgModule } from '@angular/core';
import { PreloadAllModules, RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/business/guards/auth.guard';
import { CurrentUserLoadedGuard } from 'src/business/guards/current-user-loaded.guard';
import { AppContainerComponent } from './app-container/app-container.component';

const routes: Routes = [
    { path: 'auth', loadChildren: () => import('src/app/features/authorization/auth.module').then(mod => mod.AuthModule) },
    {
        path: '', data: { isPublic: true }, component: AppContainerComponent, children: [
            { path: '', redirectTo: 'contacts', pathMatch: 'full' },
            { path: 'contacts', data: { isPublic: true }, loadChildren: () => import('src/app/features/dashboard/dashboard.module').then(mod => mod.DashboardModule) },
        ]
    },
    {
        path: 'app', canActivate: [CurrentUserLoadedGuard], children: [
            {
                path: '', component: AppContainerComponent, canActivate: [AuthGuard], children: [
                    { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
                    { path: 'dashboard', data: { isPublic: false }, loadChildren: () => import('src/app/features/dashboard/dashboard.module').then(mod => mod.DashboardModule) },
                ]
            },
            {
                path: 'settings', component: AppContainerComponent, canActivate: [AuthGuard], children: [
                ],
            },
        ]
    },
    { path: '**', redirectTo: '/' }
];

@NgModule({
    imports: [
        RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules, enableTracing: false })
    ],
    exports: [
        RouterModule
    ],
    providers: [
    ]
})
export class CoreRoutingModule {
}
