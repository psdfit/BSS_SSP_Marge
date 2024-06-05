import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InceptionReportChangeRequestApprovalsComponent } from './cr-inception-report-approvals.component';

describe('InceptionReportChangeRequestApprovalsComponent', () => {
  let component: InceptionReportChangeRequestApprovalsComponent;
  let fixture: ComponentFixture<InceptionReportChangeRequestApprovalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [InceptionReportChangeRequestApprovalsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InceptionReportChangeRequestApprovalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
