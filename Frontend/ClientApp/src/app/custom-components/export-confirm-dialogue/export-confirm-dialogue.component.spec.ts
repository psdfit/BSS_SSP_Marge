import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ExportConfirmDialogueComponent } from './export-confirm-dialogue.component';

describe('ExportConfirmDialogueComponent', () => {
  let component: ExportConfirmDialogueComponent;
  let fixture: ComponentFixture<ExportConfirmDialogueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ExportConfirmDialogueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ExportConfirmDialogueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
