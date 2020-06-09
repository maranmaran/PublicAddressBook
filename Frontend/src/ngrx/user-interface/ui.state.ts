import { Theme } from 'src/business/shared/theme.enum';
import { UIProgressBar } from 'src/business/shared/ui-progress-bars.enum';

export interface UIState {
    language: string,
    theme: Theme,
    showErrorDialogs: boolean,
    httpRequestLoading: boolean,
    httpErrorMessage: string,
    activeProgressBar: UIProgressBar,
}

export const initialUIState: UIState = {
    language: "en",
    theme: Theme.Light,
    showErrorDialogs: true,
    httpRequestLoading: false,
    httpErrorMessage: undefined,
    activeProgressBar: UIProgressBar.MainAppScreen,
};
