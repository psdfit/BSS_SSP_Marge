import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AnnualPlanApprovalComponent } from './annual-plan-approval.component';

describe('AnnualPlanApprovalComponent', () => {
  let component: AnnualPlanApprovalComponent;
  let fixture: ComponentFixture<AnnualPlanApprovalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AnnualPlanApprovalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AnnualPlanApprovalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
