import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VoilationSummaryReportComponent } from './voilation-summary-report.component';

describe('VoilationSummaryReportComponent', () => {
  let component: VoilationSummaryReportComponent;
  let fixture: ComponentFixture<VoilationSummaryReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VoilationSummaryReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VoilationSummaryReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
