import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BiometricEnrollmentDialogComponent } from './biometric-enrollment-dialog.component';

describe('BiometricEnrollmentDialogComponent', () => {
  let component: BiometricEnrollmentDialogComponent;
  let fixture: ComponentFixture<BiometricEnrollmentDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BiometricEnrollmentDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BiometricEnrollmentDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
