import { createReducer, on } from '@ngrx/store';
import { Action, ActionReducer } from '@ngrx/store/src/models';
import { UIProgressBar } from 'src/business/shared/ui-progress-bars.enum';
import { Theme } from '../../business/shared/theme.enum';
import * as UIActions from './ui.actions';
import { initialUIState, UIState } from './ui.state';

export const uiReducer: ActionReducer<UIState, Action> = createReducer(
    initialUIState,

    on(UIActions.setLanguage, (state: UIState, payload: { language: string }) => {
        return {
            ...state,
            language: payload.language
        }
    }),

    // error snackbar
    on(UIActions.disableErrorDialogs, (state: UIState) => {
        return {
            ...state,
            showErrorDialogs: false
        }
    }),
    on(UIActions.enableErrorDialogs, (state: UIState) => {
        return {
            ...state,
            showErrorDialogs: true
        }
    }),

    // http requests loading
    on(UIActions.httpRequestStartLoading, (state: UIState) => {
        return {
            ...state,
            httpRequestLoading: true,
        }
    }),
    on(UIActions.httpRequestStopLoading, (state: UIState) => {
        return {
            ...state,
            httpRequestLoading: false,
        }
    }),

    // set progress bar which is active
    on(UIActions.setActiveProgressBar, (state: UIState, payload: { progressBar: UIProgressBar }) => {
        return {
            ...state,
            activeProgressBar: payload.progressBar,
        }
    }),

    // theme switching
    on(UIActions.switchTheme, (state: UIState, payload: { theme: Theme }) => {
        return {
            ...state,
            theme: payload.theme,
        }
    }),

);
