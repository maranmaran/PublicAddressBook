import { Contact } from './contact.model';

export class PhoneNumber {
    id: string;
    number: string;

    contactId: string;
    contact: Contact;
}