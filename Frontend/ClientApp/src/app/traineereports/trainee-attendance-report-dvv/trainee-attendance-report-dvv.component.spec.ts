import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TraineeAttendanceReportDVVComponent } from './trainee-attendance-report-dvv.component';

describe('TraineeStatusReportComponent', () => {
  let component: TraineeAttendanceReportDVVComponent;
  let fixture: ComponentFixture<TraineeAttendanceReportDVVComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [TraineeAttendanceReportDVVComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TraineeAttendanceReportDVVComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
