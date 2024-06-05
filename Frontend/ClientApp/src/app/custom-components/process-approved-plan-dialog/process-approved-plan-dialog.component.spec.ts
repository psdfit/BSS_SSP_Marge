import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProcessApprovedPlanDialogComponent } from './process-approved-plan-dialog.component';

describe('ProcessApprovedPlanDialogComponent', () => {
  let component: ProcessApprovedPlanDialogComponent;
  let fixture: ComponentFixture<ProcessApprovedPlanDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProcessApprovedPlanDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProcessApprovedPlanDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
