import { HttpBackend, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { AppSettingsService } from './app-settings.service';

@Injectable()
export class PhoneValidationService {

    private httpClient: HttpClient;
    private baseUrl = 'http://apilayer.net/api/validate';
    private useRegex = true;
    private regex = new RegExp("^\\+(?:[0-9]●?){6,14}[0-9]$");

    constructor(
        private httpBackend: HttpBackend,
        private appSettings: AppSettingsService
    ) {
        this.httpClient = new HttpClient(this.httpBackend);
        if (this.appSettings.numverifyAccessKey) {
            this.useRegex = false;
            this.baseUrl += '?access_key=' + this.appSettings.numverifyAccessKey;
        }
    }

    validateNumber(number: string) {
        if (this.useRegex == false) {
            return this.httpClient.get<PhoneValidationResult>(`${this.baseUrl}&number=` + number.trim() + '&country_code=&format=1');
        } else {
            let result = new PhoneValidationResult();
            result.valid = number.trim().match(this.regex).length > 0;
            return of(result);
        }
    }
}

export class PhoneValidationResult {
    valid: boolean;
    number: string;
    local_format: string;
    international_format: string;
    country_prefix: string;
    country_code: string;
    country_name: string;
    location: string;
    carrier: string;
    line_type: string;
}