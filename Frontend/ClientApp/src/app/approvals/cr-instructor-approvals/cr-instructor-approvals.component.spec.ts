import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InstructorChangeRequestApprovalsComponent } from './cr-instructor-approvals.component';

describe('InstructorChangeRequestApprovalsComponent', () => {
    let component: InstructorChangeRequestApprovalsComponent;
    let fixture: ComponentFixture<InstructorChangeRequestApprovalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [InstructorChangeRequestApprovalsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(InstructorChangeRequestApprovalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
