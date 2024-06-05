import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SrnApprovalsDialogueComponent } from './srn-approvals-dialogue.component';

describe('SrnApprovalsDialogueComponent', () => {
  let component: SrnApprovalsDialogueComponent;
  let fixture: ComponentFixture<SrnApprovalsDialogueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SrnApprovalsDialogueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SrnApprovalsDialogueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
