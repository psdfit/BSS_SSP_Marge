import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TraineeEnrollmentReportDVVComponent } from './trainee-enrollment-report-dvv.component';

describe('TraineeStatusReportComponent', () => {
  let component: TraineeEnrollmentReportDVVComponent;
  let fixture: ComponentFixture<TraineeEnrollmentReportDVVComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TraineeEnrollmentReportDVVComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TraineeEnrollmentReportDVVComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
