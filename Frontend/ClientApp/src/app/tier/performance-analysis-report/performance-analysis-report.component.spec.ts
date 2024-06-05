import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PerformanceAnalysisReportComponent } from './performance-analysis-report.component';

describe('PerformanceAnalysisReportComponent', () => {
  let component: PerformanceAnalysisReportComponent;
  let fixture: ComponentFixture<PerformanceAnalysisReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PerformanceAnalysisReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PerformanceAnalysisReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
