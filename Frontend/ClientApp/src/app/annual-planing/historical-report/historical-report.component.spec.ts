import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { HistoricalReportComponent } from './historical-report.component';

describe('HistoricalReportComponent', () => {
  let component: HistoricalReportComponent;
  let fixture: ComponentFixture<HistoricalReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ HistoricalReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(HistoricalReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
