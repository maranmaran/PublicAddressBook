import { Injectable } from '@angular/core';
import { AbstractControl } from '@angular/forms';
import { timer } from 'rxjs/internal/observable/timer';
import { map, switchMap, take } from 'rxjs/operators';
import { PhoneValidationService } from './../services/shared/phone-validation.service';

@Injectable()
export class PhoneNumberValidator {


    static isValidPhoneNumber(service: PhoneValidationService) {

        return (control: AbstractControl) => {
            const phoneNumber = control.value;

            return timer(500).pipe(switchMap(_ =>
                service.validateNumber(phoneNumber)
                    .pipe(
                        take(1),
                        map(res => res.valid),
                        map(result => {

                            if (!result) {
                                return { phoneInvalid: true }
                            }

                            return null;
                        })
                    )
            ))
        }
    }
}