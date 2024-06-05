
import { CommonModule } from '@angular/common';
import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from "@angular/core";
import { NotificationRoutingModule } from './notification-routing.module';
import { NotificationsComponent } from './notifications/notifications.component';
import { NotificationDetaildialoguecomponent } from './NotificationDetail-dialogue/NotificationDetail-dialogue.component';
import { SharedModule } from '../shared/shared.module';
import { NotificationMappingComponent } from './notification-mapping/notification-mapping.component';
import { ViewAllNotificationComponent } from './view-all-notification/view-all-notification.component';


@NgModule({
  declarations: [NotificationsComponent,NotificationDetaildialoguecomponent, NotificationMappingComponent, ViewAllNotificationComponent],
  imports: [
    CommonModule,
    NotificationRoutingModule,SharedModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class NotificationModule { }
