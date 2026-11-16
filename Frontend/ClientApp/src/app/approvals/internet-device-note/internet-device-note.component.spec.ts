import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InternetDeviceNoteComponent } from './internet-device-note.component';

describe('InternetDeviceNoteComponent', () => {
  let component: InternetDeviceNoteComponent;
  let fixture: ComponentFixture<InternetDeviceNoteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InternetDeviceNoteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InternetDeviceNoteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
