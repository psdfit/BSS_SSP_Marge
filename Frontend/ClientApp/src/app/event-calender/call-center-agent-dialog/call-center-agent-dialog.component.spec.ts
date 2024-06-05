import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CallCetnerAgentDialogComponent } from './call-center-agent-dialog.component';

describe('CallCetnerAgentDialogComponent', () => {
    let component: CallCetnerAgentDialogComponent;
    let fixture: ComponentFixture<CallCetnerAgentDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [CallCetnerAgentDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(CallCetnerAgentDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
