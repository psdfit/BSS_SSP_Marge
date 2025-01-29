import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TPRNApprovalsDialogueComponent } from './tprn-approvals-dialogue.component';

describe('TPRNApprovalsDialogueComponent', () => {
  let component: TPRNApprovalsDialogueComponent;
  let fixture: ComponentFixture<TPRNApprovalsDialogueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TPRNApprovalsDialogueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TPRNApprovalsDialogueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
