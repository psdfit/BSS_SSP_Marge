import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ApprovalDialogueComponent } from './approval-dialogue.component';

describe('ApprovalDialogueComponent', () => {
  let component: ApprovalDialogueComponent;
  let fixture: ComponentFixture<ApprovalDialogueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ApprovalDialogueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ApprovalDialogueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
