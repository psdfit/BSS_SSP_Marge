import { TraineeEnrollmentComponent } from './trainee-enrollment/trainee-enrollment.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuardService } from '../security/auth-guard.service';
import { DeviceRegistrationComponent } from './device-registration/device-registration.component';
import { TraineeAttendanceComponent } from './trainee-attendance/trainee-attendance.component';
import { DeviceStatusUpdateDialogComponent } from './device-status-update-dialog/device-status-update-dialog.component';


const routes: Routes = [
  {
    path: "device-registration",
    component:DeviceRegistrationComponent ,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Device Registration",
    },
  },
  {
    path: "manage-device-status",
    component:DeviceStatusUpdateDialogComponent ,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Manage Device Status",
    },
  },
  {
    path: "trainee-enrollment",
    component:TraineeEnrollmentComponent ,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Trainee Enrollment",
    },
  },
  {
    path: "trainee-attendance",
    component:TraineeAttendanceComponent ,
    canActivate: [AuthGuardService],
    data: {
      icon: "verified_user",
      inMenu: true,
      title: "Trainee Attendance",
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DeviceManagementRoutingModule { }
