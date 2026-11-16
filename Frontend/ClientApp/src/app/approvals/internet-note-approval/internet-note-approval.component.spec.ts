import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InternetNoteApprovalComponent } from './internet-note-approval.component';

describe('InternetNoteApprovalComponent', () => {
  let component: InternetNoteApprovalComponent;
  let fixture: ComponentFixture<InternetNoteApprovalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InternetNoteApprovalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InternetNoteApprovalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
