import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ValidateBusinessPlanComponent } from './validate-business-plan.component';

describe('ValidateBusinessPlanComponent', () => {
  let component: ValidateBusinessPlanComponent;
  let fixture: ComponentFixture<ValidateBusinessPlanComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ValidateBusinessPlanComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ValidateBusinessPlanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
