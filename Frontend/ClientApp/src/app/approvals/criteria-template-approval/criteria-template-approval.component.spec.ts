import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CriteriaTemplateApprovalComponent } from './criteria-template-approval.component';

describe('CriteriaTemplateApprovalComponent', () => {
  let component: CriteriaTemplateApprovalComponent;
  let fixture: ComponentFixture<CriteriaTemplateApprovalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CriteriaTemplateApprovalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CriteriaTemplateApprovalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
