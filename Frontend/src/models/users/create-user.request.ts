import { AccountType } from '../enums/account-type.enum';
import { Gender } from '../enums/gender.enum';

export class CreateUserRequest {
    accountType: AccountType;
    username: string;
    email: string;
    firstname: string;
    lastname: string;
    gender: Gender;

    constructor(data: Partial<CreateUserRequest>) {
        Object.assign(this, data);
    }

}