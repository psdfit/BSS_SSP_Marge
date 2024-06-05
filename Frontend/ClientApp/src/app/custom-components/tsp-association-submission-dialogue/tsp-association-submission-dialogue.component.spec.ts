import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TspAssociationSubmissionDialogueComponent } from './tsp-association-submission-dialogue.component';

describe('TspAssociationSubmissionDialogueComponent', () => {
  let component: TspAssociationSubmissionDialogueComponent;
  let fixture: ComponentFixture<TspAssociationSubmissionDialogueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TspAssociationSubmissionDialogueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TspAssociationSubmissionDialogueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
