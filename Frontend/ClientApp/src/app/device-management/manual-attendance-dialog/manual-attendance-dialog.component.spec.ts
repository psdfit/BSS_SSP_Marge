import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManualAttendanceDialogComponent } from './manual-attendance-dialog.component';

describe('ManualAttendanceDialogComponent', () => {
  let component: ManualAttendanceDialogComponent;
  let fixture: ComponentFixture<ManualAttendanceDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManualAttendanceDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManualAttendanceDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
