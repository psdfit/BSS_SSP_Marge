import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClassDatesChangeRequestApprovalsComponent } from './cr-class-dates-approvals.component';

describe('ClassDatesChangeRequestApprovalsComponent', () => {
  let component: ClassDatesChangeRequestApprovalsComponent;
  let fixture: ComponentFixture<ClassDatesChangeRequestApprovalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ClassDatesChangeRequestApprovalsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClassDatesChangeRequestApprovalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
