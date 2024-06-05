import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PrnReportComponent } from './prn-report.component';

describe('PrnReportComponent', () => {
  let component: PrnReportComponent;
  let fixture: ComponentFixture<PrnReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [PrnReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PrnReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
