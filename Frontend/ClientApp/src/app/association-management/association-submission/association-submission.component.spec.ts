import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociationSubmissionComponent } from './association-submission.component';

describe('AssociationSubmissionComponent', () => {
  let component: AssociationSubmissionComponent;
  let fixture: ComponentFixture<AssociationSubmissionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociationSubmissionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociationSubmissionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
