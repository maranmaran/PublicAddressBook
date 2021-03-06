import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError } from 'rxjs/operators';
import { ApplicationUser } from 'src/models/domain-entities/application-user.model';
import { UserSetting } from 'src/models/domain-entities/user-settings.model';
import { AccountType } from 'src/models/enums/account-type.enum';
import { CrudService } from '../crud.service';


@Injectable({ providedIn: 'root' })
export class UserService extends CrudService<ApplicationUser> {

    constructor(
        private httpDI: HttpClient,
    ) {
        super(httpDI, "Users");
    }

    public getAll(accountType?: AccountType, coachId?: string) {
        return this.http.get<ApplicationUser[]>(this.url + 'GetAll/' + accountType + '/' + coachId)
            .pipe(
                catchError(this.handleError)
            );
    }

    public getOne(id?: string, accountType?: AccountType) {
        return this.http.get<ApplicationUser>(this.url + 'Get/' + id + '/' + accountType)
            .pipe(
                catchError(this.handleError)
            );
    }

    public delete(id: string, accountType?: AccountType) {
        return this.http.delete(this.url + 'Delete/' + id + '/' + accountType)
            .pipe(
                catchError(this.handleError)
            );
    }

    public setActive(id: string, active: boolean) {
        return this.http.get(this.url + 'SetActive/' + id + '/' + active)
            .pipe(
                catchError(this.handleError)
            );
    }

    public saveSettings(userSetting: UserSetting) {
        const payload = { userSetting: userSetting };
        return this.http.post(this.url + 'SaveuserSetting/', payload)
            .pipe(
                catchError(this.handleError)
            );
    }

    public uploadAvatar(userId: string, base64: string) {

        const formData: FormData = new FormData();
        formData.append('userId', userId);
        formData.append('base64Image', base64);

        return this.http
            .post('Media/UploadAvatar/', formData, { responseType: 'text' })
            .pipe(catchError(this.handleError));
    }


}
