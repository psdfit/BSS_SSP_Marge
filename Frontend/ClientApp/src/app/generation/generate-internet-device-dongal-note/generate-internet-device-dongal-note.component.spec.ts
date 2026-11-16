import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GenerateInternetDeviceDongalNoteComponent } from './generate-internet-device-dongal-note.component';

describe('GenerateInternetDeviceDongalNoteComponent', () => {
  let component: GenerateInternetDeviceDongalNoteComponent;
  let fixture: ComponentFixture<GenerateInternetDeviceDongalNoteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GenerateInternetDeviceDongalNoteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GenerateInternetDeviceDongalNoteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
