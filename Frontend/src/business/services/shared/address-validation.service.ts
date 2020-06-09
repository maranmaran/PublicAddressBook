// import { HttpBackend, HttpClient } from '@angular/common/http';
// import { Injectable } from '@angular/core';
// import { of } from 'rxjs';
// import { AppSettingsService } from './app-settings.service';

// @Injectable()
// export class AddressValidationService {

//     private httpClient: HttpClient;
//     private baseUrl = 'https://neutrinoapi.net/geocode-address';

//     constructor(
//         private httpBackend: HttpBackend,
//         private appSettings: AppSettingsService
//     ) {
//         this.httpClient = new HttpClient(this.httpBackend);
//         this.baseUrl += '?user_id=' + this.appSettings.addressValidationServiceUserID;
//         this.baseUrl += '&api_key=' + this.appSettings.addressValidationServiceAPIKey;
//     }

//     validateAddress(address: string) {
//         if (!address || address.trim() == "")
//             return of({ found: 0 });

//         return this.httpClient.get<AddressValidationResult>(`${this.baseUrl}&address=` + address.trim());
//     }
// }

// export class AddressValidationResult {
//     found: number;
//     locations: any[];
// }
