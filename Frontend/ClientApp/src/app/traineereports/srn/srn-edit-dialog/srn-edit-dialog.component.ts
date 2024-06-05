import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-srn-edit-dialog',
  templateUrl: './srn-edit-dialog.component.html',
  styleUrls: ['./srn-edit-dialog.component.scss']
})
export class SrnEditDialogComponent implements OnInit {

  srnForm: FormGroup;
  constructor(
    private fb: FormBuilder,
    public dialogRef: MatDialogRef<SrnEditDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: any) {

    this.srnForm = this.fb.group({
      SRNID: data.SRNID,
      ReportId: fb.control({ value: data.ReportId, disabled: true }),
      //Amount: '',
      TokenNumber: data.TokenNumber,
      TransactionNumber: data.TransactionNumber,
      IsPaid: data.IsPaid,
      // IsVarified: '',
      // Month: '',
      // NumberOfMonths: '',
      // TraineeName: data.TraineeName,
      // TraineeCNIC: '',
      // FatherName: '',
      // ContactNumber1: ''
    })
  }

  ngOnInit() {
  }

  onNoClick(): void {
    this.dialogRef.close(false);
  }
  onSaveClick(): void {
    this.dialogRef.close(this.srnForm.getRawValue())
  }
}
