import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";

import { DeviceManagementRoutingModule } from "./device-management-routing.module";
import { DeviceRegistrationComponent } from "./device-registration/device-registration.component";
import { ManageDeviceStatusComponent } from "./manage-device-status/manage-device-status.component";
import { TraineeEnrollmentComponent } from "./trainee-enrollment/trainee-enrollment.component";
import { TraineeAttendanceComponent } from "./trainee-attendance/trainee-attendance.component";
import { BiometricEnrollmentDialogComponent } from "./biometric-enrollment-dialog/biometric-enrollment-dialog.component";
import { BiometricAttendanceDialogComponent } from "./biometric-attendance-dialog/biometric-attendance-dialog.component";
import { ManualAttendanceDialogComponent } from "./manual-attendance-dialog/manual-attendance-dialog.component";
import { SharedModule } from "../shared/shared.module";
import { DeviceStatusUpdateDialogComponent } from './device-status-update-dialog/device-status-update-dialog.component';

@NgModule({
  declarations: [
    DeviceRegistrationComponent,
    ManageDeviceStatusComponent,
    TraineeEnrollmentComponent,
    TraineeAttendanceComponent,
    BiometricEnrollmentDialogComponent,
    BiometricAttendanceDialogComponent,
    ManualAttendanceDialogComponent,
    DeviceStatusUpdateDialogComponent,
  ],
  imports: [CommonModule,SharedModule, DeviceManagementRoutingModule],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class DeviceManagementModule {}
