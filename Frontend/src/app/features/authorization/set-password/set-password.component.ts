import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Store } from '@ngrx/store';
import { take } from 'rxjs/operators';
import { AuthService } from 'src/business/services/features/auth.service';
import { PasswordValidation } from 'src/business/validators/password.validator';
import { CurrentUser } from 'src/models/authorization/current-user.model';
import { SetPasswordRequest } from 'src/models/authorization/set-password.request';
import { loginSuccess } from 'src/ngrx/auth/auth.actions';
import { AppState } from 'src/ngrx/global-setup.ngrx';


@Component({
  selector: 'app-set-password',
  templateUrl: './set-password.component.html',
  styleUrls: ['./set-password.component.scss']
})
export class SetPasswordComponent implements OnInit {

  setPasswordForm: FormGroup;
  private userId: string;

  constructor(
    private store: Store<AppState>,
    private authService: AuthService,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.userId = this.route.snapshot.params.id
    this.createForm();
  }

  private createForm() {
    this.setPasswordForm = new FormGroup({
      password: new FormControl('', Validators.required),
      repeatPassword: new FormControl('', Validators.required),
    }, {
      validators: PasswordValidation.MatchPassword
    });
  }

  public onSubmit() {
    if (this.setPasswordForm.valid) {

      const password = this.setPasswordForm.get('password').value;
      const repeatPassword = this.setPasswordForm.get('repeatPassword').value;

      const request = new SetPasswordRequest(this.userId, password, repeatPassword);
      this.authService.setPassword(request).pipe(take(1))
        .subscribe(
          (user: CurrentUser) => this.store.dispatch(loginSuccess({ user })),
          err => console.log(err)
        )
    }
  }
}
