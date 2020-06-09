import { UserSetting } from '../domain-entities/user-settings.model';
import { AccountType } from '../enums/account-type.enum';
import { Gender } from '../enums/gender.enum';

export interface CurrentUser {
    id: string;

    username: string;
    email: string;

    firstName: string;
    lastName: string;
    fullName: string;
    gender: Gender;

    avatar: string;

    accountType: AccountType;

    active: boolean;
    userSetting: UserSetting;
}
