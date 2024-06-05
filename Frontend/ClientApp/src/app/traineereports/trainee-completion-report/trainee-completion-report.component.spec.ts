import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TraineeCompletionReportComponent } from './trainee-completion-report.component';

describe('TraineeCompletionReportComponent', () => {
  let component: TraineeCompletionReportComponent;
  let fixture: ComponentFixture<TraineeCompletionReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TraineeCompletionReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TraineeCompletionReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
