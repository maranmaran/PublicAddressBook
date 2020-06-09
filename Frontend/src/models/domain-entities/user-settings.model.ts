import { Theme } from 'src/business/shared/theme.enum';
import { NotificationType } from '../enums/notification-type.enum';

export class UserSetting {
    id: string;
    theme: Theme;
    notificationSettings: NotificationSetting[];
    language: string;
}

export class NotificationSetting {
    id: string;
    notificationType: NotificationType;
    receiveMail: boolean;
    receiveNotification: boolean;
}
