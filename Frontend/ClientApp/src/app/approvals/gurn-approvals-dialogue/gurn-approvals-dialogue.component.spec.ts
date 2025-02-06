import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GurnApprovalsDialogueComponent } from './gurn-approvals-dialogue.component';

describe('SrnApprovalsDialogueComponent', () => {
  let component: GurnApprovalsDialogueComponent;
  let fixture: ComponentFixture<GurnApprovalsDialogueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GurnApprovalsDialogueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GurnApprovalsDialogueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
