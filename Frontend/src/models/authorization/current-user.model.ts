import { Subscription } from 'src/models/stripe/subscription.model';
import { UserSetting } from '../domain-entities/user-settings.model';
import { AccountType } from '../enums/account-type.enum';
import { Gender } from '../enums/gender.enum';
import { SubscriptionStatus } from '../enums/subscription-status.enum';
import { Plan } from '../stripe/plan.model';
import { StripeList } from '../stripe/stripe-list.model';

export interface CurrentUser {
    id: string;
    customerId: string;

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

    splashDialogDate: string;

    subscriptionStatus: SubscriptionStatus;
    subscriptionInfo: Subscription;
    plans: StripeList<Plan>;
    trialDaysRemaining: number;
}
