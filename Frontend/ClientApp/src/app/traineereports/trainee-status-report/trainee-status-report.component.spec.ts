import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TraineeStatusReportComponent } from './trainee-status-report.component';

describe('TraineeStatusReportComponent', () => {
  let component: TraineeStatusReportComponent;
  let fixture: ComponentFixture<TraineeStatusReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TraineeStatusReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TraineeStatusReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
