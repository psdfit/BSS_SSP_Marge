import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProcessStatusUpdateDialogComponent } from './process-status-update-dialog.component';

describe('ProcessStatusUpdateDialogComponent', () => {
  let component: ProcessStatusUpdateDialogComponent;
  let fixture: ComponentFixture<ProcessStatusUpdateDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProcessStatusUpdateDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProcessStatusUpdateDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
