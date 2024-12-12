import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DeviceStatusUpdateDialogComponent } from './device-status-update-dialog.component';

describe('DeviceStatusUpdateDialogComponent', () => {
  let component: DeviceStatusUpdateDialogComponent;
  let fixture: ComponentFixture<DeviceStatusUpdateDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DeviceStatusUpdateDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DeviceStatusUpdateDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
