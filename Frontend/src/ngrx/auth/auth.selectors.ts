import { createFeatureSelector, createSelector } from '@ngrx/store';
import { CurrentUser } from 'src/models/authorization/current-user.model';
import { AccountType } from 'src/models/enums/account-type.enum';
import { SubscriptionStatus } from 'src/models/enums/subscription-status.enum';
import { AuthState } from './auth.state';

export const selectAuthState = createFeatureSelector<AuthState>("auth");

export const currentUser = createSelector(
    selectAuthState,
    (authState: AuthState) => authState.currentUser
);

export const currentUserId = createSelector(
    selectAuthState,
    (authState: AuthState) => authState.currentUser.id
);

export const userSetting = createSelector(
    currentUser,
    (currentUser: CurrentUser) => currentUser.userSetting
);


export const isLoggedIn = createSelector(
    selectAuthState,
    (authState: AuthState) => !!authState.currentUser
);

export const isLoggedOut = createSelector(
    isLoggedIn,
    (loggedIn: boolean) => !loggedIn
);

export const trialDaysRemaining = createSelector(
    currentUser,
    (currentUser: CurrentUser) => currentUser.trialDaysRemaining
);

export const subscriptionStatus = createSelector(
    currentUser,
    (currentUser: CurrentUser) => currentUser.subscriptionStatus
);

export const isSubscribed = createSelector(
    subscriptionStatus,
    (subStatus: SubscriptionStatus) => {

        switch (subStatus.toString().toLowerCase()) {
            case SubscriptionStatus.active:
                return true;
            default:
                return false;
        }
    }
);

export const isTrialing = createSelector(
    trialDaysRemaining,
    (daysRemaining: number) => daysRemaining > 0 ? true : false
);

export const isCoachOrSoloAthlete = createSelector(currentUser, (user: CurrentUser) => user?.accountType == AccountType.Coach || user?.accountType == AccountType.Admin || user?.accountType == AccountType.SoloAthlete);
export const isCoach = createSelector(currentUser, (user: CurrentUser) => user?.accountType == AccountType.Coach)
export const isSoloAthlete = createSelector(currentUser, (user: CurrentUser) => user?.accountType == AccountType.SoloAthlete)
export const isAthlete = createSelector(currentUser, (user: CurrentUser) => user?.accountType == AccountType.Athlete)

export const isPayingUser = createSelector(
    currentUser,
    (user: CurrentUser) => {
        if (user) {
            const accountType = user.accountType;
            return accountType == AccountType.Coach || accountType == AccountType.SoloAthlete || accountType == AccountType.Admin;
        }
    }
);

export const loginError = createSelector(
    selectAuthState,
    (state: AuthState) => state.loginError
);
