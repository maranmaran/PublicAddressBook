import { createAction, props } from '@ngrx/store';
import { CurrentUser } from 'src/models/authorization/current-user.model';
import { SignInRequest } from 'src/models/authorization/sign-in.request';
import { UserSetting } from 'src/models/domain-entities/user-settings.model';
import { Subscription } from 'src/models/stripe/subscription.model';

//#region Login actions

export const login = createAction(
    '[Auth API] Login',
    props<{ request: SignInRequest }>()
)

export const loginSuccess = createAction(
    '[Auth API] Login Success',
    props<{ user: CurrentUser }>()
)

export const loginFailure = createAction(
    '[Auth API] Login Failure',
    props<{ error: Error }>()
)

export const logout = createAction(
    '[Auth API] Logout'
)
export const logoutClearState = createAction(
    '[Auth API] Clear state - logout'
)
//#endregion

//#region Current user actions

export const fetchCurrentUser = createAction(
    '[Auth API] Fetch current user'
)

export const updateCurrentUser = createAction(
    '[Auth API] Update current user',
    props<{ user: CurrentUser }>()
)

//#endregion

//#region Current user modifier actions


export const updateUserSetting = createAction(
    '[User API] Update user settings',
    props<{ settings: UserSetting }>()
)

export const settingsUpdated = createAction(
    '[User API] User settings - UPDATED',
    props<{ settings: UserSetting }>()
)

export const addSubscription = createAction(
    '[Billing API] Add subscription',
    props<{ subscription: Subscription }>()
)

export const cancelSubscription = createAction(
    '[Billing API] Cancel subscription'
)

//#endregion
