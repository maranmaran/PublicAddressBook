// import { Injectable } from '@angular/core';
// import { AbstractControl } from '@angular/forms';
// import { timer } from 'rxjs/internal/observable/timer';
// import { map, switchMap, take } from 'rxjs/operators';
// import { AddressValidationService } from './../services/shared/address-validation.service';

// @Injectable()
// export class AddressValidator {


//     static isValidAddress(service: AddressValidationService) {

//         return (control: AbstractControl) => {
//             const address = control.value;

//             return timer(500).pipe(switchMap(_ =>
//                 service.validateAddress(address)
//                     .pipe(
//                         take(1),
//                         map(res => res.found > 0),
//                         map(result => {

//                             if (!result) {
//                                 return { addressInvalid: true }
//                             }

//                             return null;
//                         })
//                     )
//             ))
//         }
//     }
// }