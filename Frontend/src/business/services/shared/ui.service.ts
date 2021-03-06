import { ComponentType, OverlayContainer } from '@angular/cdk/overlay';
import { Injectable } from '@angular/core';
import { MatDialog, MatDialogConfig, MatDialogRef } from '@angular/material/dialog';
import { MatSidenav } from '@angular/material/sidenav';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Store } from '@ngrx/store';
import { Observable, of } from 'rxjs';
import { take } from 'rxjs/operators';
import { ConfirmDialogComponent } from 'src/app/shared/dialogs/confirm-dialog/confirm-dialog.component';
import { MessageDialogComponent } from 'src/app/shared/dialogs/message-dialog/message-dialog.component';
import { snackBarDefaultConfig } from 'src/business/shared/snackbar.config';
import { getThemeClass, Theme } from 'src/business/shared/theme.enum';
import { UISidenav, UISidenavAction } from 'src/business/shared/ui-sidenavs.enum';
import { Dictionary } from 'src/business/utils/dictionary';
import { AppState } from 'src/ngrx/global-setup.ngrx';
import { ConfirmDialogConfig, ConfirmResult } from './../../shared/confirm-dialog.config';

@Injectable()
export class UIService {

    constructor(
        private store: Store<AppState>,
        private snackBar: MatSnackBar,
        private dialog: MatDialog,
        private overlayContainer: OverlayContainer,
    ) { }


    // #region ================ DIALOGS ================

    public openDialogFromComponent(component: ComponentType<any>, config?: MatDialogConfig,
        callbackAction?: Function, callbackActionParams?: any[]) {

        const opts = Object.assign({}, snackBarDefaultConfig, config);

        const dialogRef = this.dialog.open(component, opts);

        dialogRef.afterClosed()
            .pipe(take(1))
            .subscribe((params) => {
                if (params != ConfirmResult.Reject) {
                    callbackAction && callbackAction(...callbackActionParams);
                }
            });

        return dialogRef;
    }

    //TODO: Make confirm dialog config object with all the params
    confirmDialog: MatDialogRef<any, any>;
    public openConfirmDialog(config: ConfirmDialogConfig) {

        if (this.confirmDialog) this.confirmDialog.close();

        return this.openDialogFromComponent(ConfirmDialogComponent, {
            height: 'auto',
            maxWidth: '20rem',
            autoFocus: false,
            disableClose: !config.allowConfirm,
            closeOnNavigation: config.allowConfirm,
            data: { config },
        }, config.action, config.actionParams);
    }

    private _fadeOutMessageDialog: MatDialogRef<MessageDialogComponent>;
    public fadeOutMessage(message: string, timeout: number = 1000) {

        this._fadeOutMessageDialog && this._fadeOutMessageDialog.close();

        this._fadeOutMessageDialog = this.dialog.open(MessageDialogComponent, {
            height: 'auto',
            maxWidth: '20rem',
            autoFocus: false,
            disableClose: true,
            data: { message: message },
        });

        setTimeout(() => {
            this._fadeOutMessageDialog.close();
        }, timeout);
    }

    // #endregion

    // #region ================ THEME ================

    public setupTheme(theme: Theme) {

        const overlayContainer = this.overlayContainer.getContainerElement();
        const documentBody = document.getElementsByTagName("BODY")[0];

        const overlayContainerClasses = overlayContainer.classList;
        const bodyClasses = documentBody.classList;

        const overlayThemeClassesToRemove = Array.from(overlayContainerClasses).filter((item: string) => item.includes('theme-'));
        const bodyThemeClassesToRemove = Array.from(bodyClasses).filter((item: string) => item.includes('theme-'));

        overlayThemeClassesToRemove.length && overlayContainerClasses.remove(...overlayThemeClassesToRemove);
        bodyThemeClassesToRemove.length && bodyClasses.remove(...bodyThemeClassesToRemove);

        var themeClass = getThemeClass(theme);
        overlayContainerClasses.add(themeClass);
        bodyClasses.add(themeClass);

    }

    // #endregion

    // #region ================ SIDEBARS ================

    private sidenavs = new Dictionary<MatSidenav>();

    public addOrUpdateSidenav(name: UISidenav, sidenav: MatSidenav) {
        this.sidenavs.addOrUpdate(name, sidenav);
    }

    public doSidenavAction(name: UISidenav, actionType: UISidenavAction) {

        var sidenav = this.sidenavs.item(name);

        if (!sidenav) {
            console.warn('Sidenav isn\'t registered ');
            return;
        }

        switch (actionType) {
            case UISidenavAction.Open:
                sidenav.mode = 'side';
                sidenav.open();
                //showMenuButton
                break;

            case UISidenavAction.Toggle:
                sidenav.toggle();
                //showMenuButton
                break;

            case UISidenavAction.Close:
                sidenav.mode = 'over';
                sidenav.close();
                //this.showSettingsMenuButton = true;
                break;

            default:
                break;
        }
    }

    public isSidenavOpened(name: string, nonObservable = false): Observable<boolean> | boolean {
        if (nonObservable)
            return this.sidenavs.item(name).opened;

        return of(this.sidenavs.item(name).opened);
    }


    // #endregion

}
