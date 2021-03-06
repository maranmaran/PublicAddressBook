import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, Validators } from '@angular/forms';
import { Store } from '@ngrx/store';
import { ImageCroppedEvent } from 'ngx-image-cropper/public-api';
import { finalize, take } from 'rxjs/operators';
import { ImageCropperComponent, ImageCropperConfiguration } from 'src/app/shared/media/image-cropper/image-cropper.component';
import { UserService } from 'src/business/services/features/user.service';
import { UIService } from 'src/business/services/shared/ui.service';
import { validateForm } from 'src/business/utils/form.utils';
import { CurrentUser } from 'src/models/authorization/current-user.model';
import { UpdateUserRequest } from 'src/models/users/update-user.request';
import { updateCurrentUser } from 'src/ngrx/auth/auth.actions';
import { currentUser } from 'src/ngrx/auth/auth.selectors';
import { AppState } from 'src/ngrx/global-setup.ngrx';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.scss']
})
export class AccountComponent implements OnInit {

  accountForm: FormGroup;
  currentUser: CurrentUser;

  // readonly inputs at first
  editUsername = true;
  editMail = true;
  editAvatar = false;

  config = new ImageCropperConfiguration({})

  constructor(
    private UIService: UIService,
    private usersService: UserService,
    private store: Store<AppState>
  ) { }

  ngOnInit() {
    this.store.select(currentUser)
      .pipe(take(1))
      .subscribe((user: CurrentUser) => {
        this.currentUser = { ...user };

        // for better resolution
        if (this.currentUser.avatar.indexOf('ui-avatars') !== -1)
          this.currentUser.avatar += '&size=300px'

        this.createForm(this.currentUser);
      });
  }

  private createForm(model: CurrentUser) {
    this.accountForm = new FormGroup({
      username: new FormControl(model.username, [Validators.required]),
      email: new FormControl(model.email, [Validators.required]),
    });
  }

  get username(): AbstractControl { return this.accountForm.get('username'); }
  get email(): AbstractControl { return this.accountForm.get('email'); }

  get getErrorMessage(): string {
    return 'Not valid';
  }

  changePassword() {
    if (!this.email.valid) return;

    // hit change password api
    console.error("Call to backend not implemented.")
  }

  onSubmit() {
    if (validateForm(this.accountForm, this.UIService)) {

      const request = new UpdateUserRequest({
        accountType: this.currentUser.accountType,
        id: this.currentUser.id,
        username: this.username.value,
        email: this.email.value,
        firstname: this.currentUser.firstName,
        lastname: this.currentUser.lastName,
        gender: this.currentUser.gender,
        active: this.currentUser.active,
      });

      this.usersService.update(request)
        .pipe(
          take(1),
          finalize(() => {
            this.editUsername = true;
            this.editMail = true;
          }))
        .subscribe(
          () => {
            this.currentUser.username = this.username.value;
            this.currentUser.email = this.email.value;

            // set new state but don't pass by reference
            console.log('Update user')
            this.store.dispatch(updateCurrentUser({ user: Object.assign({}, this.currentUser) }));
          },
          err => console.log(err),
        );
    }
  }

  fileChangeEvent(event) {
    this.config.imageChangedEvent = event;

    const dialogRef = this.UIService.openDialogFromComponent(ImageCropperComponent, {
      height: 'auto',
      width: '98%',
      maxWidth: '45rem',
      autoFocus: false,
      data: { config: this.config },
      panelClass: ['image-cropper-dialog']
    });

    dialogRef.afterClosed().pipe(take(1))
      .subscribe((imageCroppedEvent: ImageCroppedEvent) => {

        this.usersService.uploadAvatar(this.currentUser.id, imageCroppedEvent.base64).pipe(take(1))
          .subscribe((presignedUrl: string) => {
            console.log(presignedUrl);
            this.currentUser.avatar = presignedUrl;
            this.store.dispatch(updateCurrentUser({ user: this.currentUser }));
          });
      });
  }

}
