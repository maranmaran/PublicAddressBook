import { CurrentUser } from 'src/models/authorization/current-user.model';

export interface AuthState {
    currentUser: CurrentUser,
    loginError: Error,
}

export const initialAuthState: AuthState = {
    currentUser: undefined,
    loginError: undefined,
};
