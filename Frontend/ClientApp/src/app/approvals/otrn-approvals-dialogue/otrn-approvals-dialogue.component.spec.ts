import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OtrnApprovalsDialogueComponent } from './otrn-approvals-dialogue.component';

describe('OtrnApprovalsDialogueComponent', () => {
  let component: OtrnApprovalsDialogueComponent;
  let fixture: ComponentFixture<OtrnApprovalsDialogueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OtrnApprovalsDialogueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OtrnApprovalsDialogueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
