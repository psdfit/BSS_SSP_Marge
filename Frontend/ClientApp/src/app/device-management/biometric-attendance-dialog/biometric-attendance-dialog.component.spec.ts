import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BiometricAttendanceDialogComponent } from './biometric-attendance-dialog.component';

describe('BiometricAttendanceDialogComponent', () => {
  let component: BiometricAttendanceDialogComponent;
  let fixture: ComponentFixture<BiometricAttendanceDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BiometricAttendanceDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BiometricAttendanceDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
