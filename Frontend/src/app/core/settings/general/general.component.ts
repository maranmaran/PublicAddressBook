import { Component, OnInit } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { MatSlideToggleChange } from '@angular/material/slide-toggle';
import { Store } from '@ngrx/store';
import _ from 'lodash';
import { Observable } from 'rxjs';
import { map, skip, take } from 'rxjs/operators';
import { CountryService } from 'src/business/services/shared/country.service';
import { Country } from 'src/business/shared/country.model';
import { SupportedLanguages } from 'src/business/shared/supported-languages.enum';
import { Theme } from 'src/business/shared/theme.enum';
import { NotificationSetting, UserSetting } from 'src/models/domain-entities/user-settings.model';
import { updateUserSetting } from 'src/ngrx/auth/auth.actions';
import { userSetting } from 'src/ngrx/auth/auth.selectors';
import { AppState } from 'src/ngrx/global-setup.ngrx';
import { setLanguage } from 'src/ngrx/user-interface/ui.actions';
import { SubSink } from 'subsink';
import { NotificationTypeLabel } from './notification-type-labels.enum';

@Component({
  selector: 'app-general',
  templateUrl: './general.component.html',
  styleUrls: ['./general.component.scss'],
  providers: [CountryService]
})
export class GeneralComponent implements OnInit {

  public userSetting: UserSetting;

  notificationTypeLabels = NotificationTypeLabel;

  supportedLanguages = SupportedLanguages;
  supportedCountryLanguages: Observable<Country[]>;
  language = new FormControl('language');
  selectedCountryLanguageFlag: string;

  subs = new SubSink();

  constructor(
    private store: Store<AppState>,
    private countryService: CountryService
  ) { }

  ngOnInit() {
    this.getSupportedLanguages();

    this.subs.add(
      // skip(1) because of initial value
      // so we don't trigger save on every open of settings
      this.language.valueChanges.pipe(skip(1)).subscribe(value => {
        this.userSetting.language = value;
        this.getCountryFlag(value);
        this.store.dispatch(updateUserSetting({ settings: _.cloneDeep(this.userSetting) }));
        this.store.dispatch(setLanguage({ language: this.userSetting.language }))
      })
    )

    this.store.select(userSetting).pipe(take(1))
      .subscribe((userSetting: UserSetting) => {
        this.userSetting = _.cloneDeep(userSetting);
        this.language.setValue(this.userSetting.language);
        this.getCountryFlag(this.userSetting.language);
      });
  }

  getSupportedLanguages() {
    this.supportedCountryLanguages = this.countryService.getCountriesByCodes('us', 'hr');
  }

  getCountryFlag(language: string) {
    this.supportedCountryLanguages.pipe(
      map(countries => {
        var country = countries.filter(c => c.languages[0].iso639_1 == language)[0];
        return country.flag;
      }
      )).subscribe(flag => this.selectedCountryLanguageFlag = flag);
  }

  get themeButtonChecked(): boolean { return this.userSetting.theme == Theme.Dark; }

  public onThemeChange(event: MatSlideToggleChange) {
    if (event.checked) this.userSetting.theme = Theme.Dark;
    if (!event.checked) this.userSetting.theme = Theme.Light;

    this.store.dispatch(updateUserSetting({ settings: _.cloneDeep(this.userSetting) }));
  }

  public onNotificationSettingCheckboxChecked(event: MatCheckboxChange, setting: NotificationSetting) {

    let settingCopy = _.cloneDeep(setting);
    switch (event.source.name) {
      case 'receiveMail':
        settingCopy.receiveMail = event.checked;
        break;
      case 'receiveNotification':
        settingCopy.receiveNotification = event.checked;
        break;
      default:
        throw new Error("No source like that defined");
    }

    this.userSetting.notificationSettings = this.userSetting.notificationSettings.map(x => x.id == settingCopy.id ? settingCopy : x);
    this.store.dispatch(updateUserSetting({ settings: _.cloneDeep(this.userSetting) }));
  }

}
