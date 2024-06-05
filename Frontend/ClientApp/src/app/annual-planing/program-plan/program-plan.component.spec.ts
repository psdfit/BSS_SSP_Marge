import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgramPlanComponent } from './program-plan.component';

describe('ProgramPlanComponent', () => {
  let component: ProgramPlanComponent;
  let fixture: ComponentFixture<ProgramPlanComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProgramPlanComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgramPlanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
