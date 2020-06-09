import { createReducer, on } from '@ngrx/store';
import { Action, ActionReducer } from '@ngrx/store/src/models';
import { CurrentUser } from 'src/models/authorization/current-user.model';
import { SubscriptionStatus } from 'src/models/enums/subscription-status.enum';
import { Subscription } from 'src/models/stripe/subscription.model';
import { UserSetting } from './../../models/domain-entities/user-settings.model';
import * as AuthActions from './auth.actions';
import { AuthState, initialAuthState } from './auth.state';

export const authReducer: ActionReducer<AuthState, Action> = createReducer(
    initialAuthState,

    on(AuthActions.loginSuccess, (state: AuthState, payload: { user: CurrentUser }) => {
        return {
            ...state,
            currentUser: payload.user
        }
    }),

    on(AuthActions.loginFailure, (state: AuthState, payload: { error: Error }) => {
        return {
            ...state,
            loginError: payload.error
        }
    }),

    on(AuthActions.updateUserSetting, (state: AuthState, payload: { settings: UserSetting }) => {
        return {
            ...state,
            currentUser: {
                ...state.currentUser,
                userSetting: payload.settings
            }
        }
    }),

    on(AuthActions.cancelSubscription, (state: AuthState) => {
        return {
            ...state,
            currentUser: {
                ...state.currentUser,
                subscriptionInfo: undefined,
                subscriptionStatus: SubscriptionStatus.canceled
            }
        };
    }),

    on(AuthActions.addSubscription, (state: AuthState, payload: { subscription: Subscription }) => {
        return {
            ...state,
            currentUser: {
                ...state.currentUser,
                subscriptionInfo: payload.subscription,
                subscriptionStatus: SubscriptionStatus[payload.subscription?.status] ?? SubscriptionStatus.unpaid
            }
        };
    }),

    on(AuthActions.updateCurrentUser, (state: AuthState, payload: { user: CurrentUser }) => {
        return {
            ...state,
            currentUser: payload.user
        };
    }),

);
