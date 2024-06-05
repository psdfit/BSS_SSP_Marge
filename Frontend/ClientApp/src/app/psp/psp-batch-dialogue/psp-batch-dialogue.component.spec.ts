import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PSPBatchDialogueComponent } from './psp-batch-dialogue.component';

describe('PSPBatchDialogueComponent', () => {
  let component: PSPBatchDialogueComponent;
  let fixture: ComponentFixture<PSPBatchDialogueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [PSPBatchDialogueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PSPBatchDialogueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
