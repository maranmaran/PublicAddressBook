import { AccountType } from '../enums/account-type.enum';
import { Gender } from '../enums/gender.enum';

export class UpdateUserRequest {

    accountType: AccountType;
    id: string;
    username: string;
    email: string;
    firstname: string;
    lastname: string;
    gender: Gender;
    active: boolean;


    constructor(data: Partial<UpdateUserRequest>) {
        Object.assign(this, data);
    }
}
