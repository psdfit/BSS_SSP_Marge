import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RTPApprovalDialogueComponent } from './rtp-approval-dialogue.component';

describe('RTPApprovalDialogueComponent', () => {
    let component: RTPApprovalDialogueComponent;
    let fixture: ComponentFixture<RTPApprovalDialogueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [RTPApprovalDialogueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(RTPApprovalDialogueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
