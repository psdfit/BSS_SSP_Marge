import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CRTraineeHistoryDialogueComponent } from './cr-trianee-history-dialogue.component';

describe('CRTraineeHistoryDialogueComponent', () => {
  let component: CRTraineeHistoryDialogueComponent;
  let fixture: ComponentFixture<CRTraineeHistoryDialogueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [CRTraineeHistoryDialogueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CRTraineeHistoryDialogueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
