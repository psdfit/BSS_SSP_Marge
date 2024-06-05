import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ComplaintHistoryDialogueComponent } from './complaint-history-dialogue.component';

describe('ComplaintHistoryDialogueComponent', () => {
  let component: ComplaintHistoryDialogueComponent;
  let fixture: ComponentFixture<ComplaintHistoryDialogueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ComplaintHistoryDialogueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ComplaintHistoryDialogueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
