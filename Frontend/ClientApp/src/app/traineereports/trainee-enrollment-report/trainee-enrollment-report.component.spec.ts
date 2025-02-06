import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TraineeEnrollmentReportComponent } from './trainee-enrollment-report.component';

describe('TraineeEnrollmentReportComponent', () => {
  let component: TraineeEnrollmentReportComponent;
  let fixture: ComponentFixture<TraineeEnrollmentReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TraineeEnrollmentReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TraineeEnrollmentReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
