import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManageDeviceStatusComponent } from './manage-device-status.component';

describe('ManageDeviceStatusComponent', () => {
  let component: ManageDeviceStatusComponent;
  let fixture: ComponentFixture<ManageDeviceStatusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManageDeviceStatusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManageDeviceStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
