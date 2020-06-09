import { HttpBackend, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AppSettingsService } from './app-settings.service';

@Injectable()
export class PhoneValidationService {

    private httpClient: HttpClient;
    private baseUrl = 'http://apilayer.net/api/validate';

    constructor(
        private httpBackend: HttpBackend,
        private appSettings: AppSettingsService
    ) {
        this.httpClient = new HttpClient(this.httpBackend);
        this.baseUrl += '?access_key=' + this.appSettings.phoneValidationServiceAPIKey;
    }

    validateNumber(number: string) {
        return this.httpClient.get<PhoneValidationResult>(`${this.baseUrl}&number=` + number.trim() + '&country_code=&format=1');
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