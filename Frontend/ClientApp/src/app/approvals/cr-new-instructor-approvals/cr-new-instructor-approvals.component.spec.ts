import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NewInstructorRequestApprovalsComponent } from './cr-new-instructor-approvals.component';

describe('InstructorChangeRequestApprovalsComponent', () => {
  let component: NewInstructorRequestApprovalsComponent;
  let fixture: ComponentFixture<NewInstructorRequestApprovalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [NewInstructorRequestApprovalsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NewInstructorRequestApprovalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
