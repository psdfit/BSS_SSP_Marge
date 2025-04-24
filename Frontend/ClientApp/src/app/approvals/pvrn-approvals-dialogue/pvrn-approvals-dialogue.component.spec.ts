import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PvrnApprovalsDialogueComponent } from './pvrn-approvals-dialogue.component';

describe('PvrnApprovalsDialogueComponent', () => {
  let component: PvrnApprovalsDialogueComponent;
  let fixture: ComponentFixture<PvrnApprovalsDialogueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PvrnApprovalsDialogueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PvrnApprovalsDialogueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
