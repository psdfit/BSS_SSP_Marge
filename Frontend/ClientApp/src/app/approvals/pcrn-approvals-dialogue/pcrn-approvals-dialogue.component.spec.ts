import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PcrnApprovalsDialogueComponent } from './pcrn-approvals-dialogue.component';

describe('PcrnApprovalsDialogueComponent', () => {
  let component: PcrnApprovalsDialogueComponent;
  let fixture: ComponentFixture<PcrnApprovalsDialogueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PcrnApprovalsDialogueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PcrnApprovalsDialogueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
