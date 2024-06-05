import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProcessApprovedPlanComponent } from './process-approved-plan.component';

describe('ProcessApprovedPlanComponent', () => {
  let component: ProcessApprovedPlanComponent;
  let fixture: ComponentFixture<ProcessApprovedPlanComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProcessApprovedPlanComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProcessApprovedPlanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
