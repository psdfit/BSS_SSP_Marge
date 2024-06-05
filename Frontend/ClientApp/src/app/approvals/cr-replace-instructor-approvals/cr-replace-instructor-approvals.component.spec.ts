import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ReplaceInstructorChangeRequestApprovalsComponent } from './cr-replace-instructor-approvals.component';

describe('ReplaceInstructorChangeRequestApprovalsComponent', () => {
  let component: ReplaceInstructorChangeRequestApprovalsComponent;
  let fixture: ComponentFixture<ReplaceInstructorChangeRequestApprovalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ReplaceInstructorChangeRequestApprovalsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ReplaceInstructorChangeRequestApprovalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
