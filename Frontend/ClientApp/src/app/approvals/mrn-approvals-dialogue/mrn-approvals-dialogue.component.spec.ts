import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MrnApprovalsDialogueComponent } from './mrn-approvals-dialogue.component';

describe('MrnApprovalsDialogueComponent', () => {
  let component: MrnApprovalsDialogueComponent;
  let fixture: ComponentFixture<MrnApprovalsDialogueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MrnApprovalsDialogueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MrnApprovalsDialogueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
