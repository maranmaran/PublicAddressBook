import { createFeatureSelector, createSelector } from '@ngrx/store';
import { CurrentUser } from 'src/models/authorization/current-user.model';
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

export const loginError = createSelector(
    selectAuthState,
    (state: AuthState) => state.loginError
);
