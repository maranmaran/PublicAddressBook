import { PhoneNumber } from './phone-number.model';

export class Contact {
    id: string;
    name: string;
    address: string;
    phoneNumbers: PhoneNumber[];
}
