import { createReducer, on } from '@ngrx/store';
import { Action, ActionReducer } from '@ngrx/store/src/models';
import { CurrentUser } from 'src/models/authorization/current-user.model';
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

    on(AuthActions.updateCurrentUser, (state: AuthState, payload: { user: CurrentUser }) => {
        return {
            ...state,
            currentUser: payload.user
        };
    }),

);
