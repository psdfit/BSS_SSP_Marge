import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TspAssociationEvaluationDialogueComponent } from './tsp-association-evaluation-dialogue.component';

describe('TspAssociationEvaluationDialogueComponent', () => {
  let component: TspAssociationEvaluationDialogueComponent;
  let fixture: ComponentFixture<TspAssociationEvaluationDialogueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TspAssociationEvaluationDialogueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TspAssociationEvaluationDialogueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
