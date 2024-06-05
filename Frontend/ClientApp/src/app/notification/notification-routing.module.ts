import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NotificationsComponent } from './notifications/notifications.component';
import { NotificationMappingComponent } from './notification-mapping/notification-mapping.component';
import { ViewAllNotificationComponent } from './view-all-notification/view-all-notification.component';

const routes: Routes = [
  {
    path: "notifications",
    component:NotificationsComponent,
   // canActivate: [AuthGuardService],
    //data: { icon: "verified_user", inMenu: true, title: "Notifications" },
  },
  {
    path: "NotificationMapping",
    component:NotificationMappingComponent,
   // canActivate: [AuthGuardService],
    //data: { icon: "verified_user", inMenu: true, title: "Notifications" },
  },
  {
    path: "viewallnotification",
    component:ViewAllNotificationComponent,
   // canActivate: [AuthGuardService],
    //data: { icon: "verified_user", inMenu: true, title: "Notifications" },
  },
  {
    path: "viewallnotification/:id",
    component:ViewAllNotificationComponent,
   // canActivate: [AuthGuardService],
    //data: { icon: "verified_user", inMenu: true, title: "Notifications" },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class NotificationRoutingModule { }
