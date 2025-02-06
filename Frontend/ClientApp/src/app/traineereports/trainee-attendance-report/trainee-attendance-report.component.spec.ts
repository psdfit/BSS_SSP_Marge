import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TraineeAttendanceReportComponent } from './trainee-attendance-report.component';

describe('TraineeAttendanceReportComponent', () => {
  let component: TraineeAttendanceReportComponent;
  let fixture: ComponentFixture<TraineeAttendanceReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TraineeAttendanceReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TraineeAttendanceReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
