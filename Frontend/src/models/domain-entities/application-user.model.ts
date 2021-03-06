import { AccountType } from '../enums/account-type.enum';
import { Gender } from './../enums/gender.enum';
import { UserSetting } from './user-settings.model';

export class ApplicationUser {
     id: string;
     customerId: string;
     username: string;
     email: string;
     passwordHash: string;

     avatar: string;
     firstName: string;
     lastName: string;
     fullName: string;

     gender: Gender = Gender.Male;

     createdOn: string;
     lastModified: string;

     active: boolean;
     accountType: AccountType;

     userSettingId?: string;
     userSetting: UserSetting;
}
