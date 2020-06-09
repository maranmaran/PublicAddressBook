import { Injectable } from '@angular/core';
import { AbstractControl } from '@angular/forms';
import { map, take } from 'rxjs/operators';
import { PhoneValidationService } from './../services/shared/phone-validation.service';

@Injectable()
export class PhoneNumberValidatior {

    constructor(
        private phoneValidation: PhoneValidationService
    ) {

    }

    isValidPhoneNumber(control: AbstractControl) {
        const phoneNumber = control.value;

        return this.phoneValidation.validateNumber(phoneNumber)
            .pipe(
                take(1),
                map(res => res.valid)
            )
            .subscribe(result => {
                if (!result) {
                    control.setErrors({ phoneInvalid: true })
                }
            });
    }
}