import { ModuleWithProviders, NgModule, Optional, SkipSelf } from '@angular/core';
import { NotificationSignalrService } from 'src/business/services/features/notification-signalr.service';

/**
 * Singleton services - this pattern is used to have singleton instances of services inside lazy loaded modules
 */
@NgModule({})
export class SignalrHubsModule {

    constructor(@Optional() @SkipSelf() parentModule?: SignalrHubsModule) {
        if (parentModule) {
            throw new Error(
                'SignalrHubsModule is already loaded. Import it in the CoreModule only');
        }
    }

    static forRoot(): ModuleWithProviders<SignalrHubsModule> {
        return {
            ngModule: SignalrHubsModule,
            providers: [
                NotificationSignalrService,
            ]
        }
    }

}
