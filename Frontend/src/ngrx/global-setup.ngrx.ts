import { ActionReducer, ActionReducerMap, MetaReducer } from '@ngrx/store';
import { environment } from 'src/environments/environment';
import { AuthEffects } from './auth/auth.effects';
import { authReducer } from './auth/auth.reducers';
import { AuthState } from './auth/auth.state';
import { clearState } from './clear-state.reducer';
import { UIEffects } from './user-interface/ui.effects';
import { uiReducer } from './user-interface/ui.reducers';
import { UIState } from './user-interface/ui.state';

export interface AppState {
  auth: AuthState,
  ui: UIState,
}

export const reducers: ActionReducerMap<AppState> = {
  auth: authReducer,
  ui: uiReducer,
};

export function stateLogger(reducer: ActionReducer<any>)
  : ActionReducer<any> {
  return (state, action) => {
    console.log("state before: ", state);
    console.log("action: ", action);

    return reducer(state, action);
  }
}

export const metaReducers: MetaReducer<AppState>[] =
  !environment.production ?
    [clearState] :
    [clearState]
// !environment.production ? [stateLogger] : [];

// export const CoreEffects = [AppInitializeEffects, AuthEffects, UIEffects];
export const CoreEffects = [AuthEffects, UIEffects];
