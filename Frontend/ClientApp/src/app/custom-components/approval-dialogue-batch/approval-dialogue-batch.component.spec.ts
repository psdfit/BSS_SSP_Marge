import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ApprovalDialogueBatchComponent } from './approval-dialogue-batch.component';

describe('ApprovalDialogueBatchComponent', () => {
  let component: ApprovalDialogueBatchComponent;
  let fixture: ComponentFixture<ApprovalDialogueBatchComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ApprovalDialogueBatchComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ApprovalDialogueBatchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
