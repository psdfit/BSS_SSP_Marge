import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VisaStampingDocDialogComponent } from './visa-stamping-doc-dialog.component';

describe('ManualAttendanceDialogComponent', () => {
  let component: VisaStampingDocDialogComponent;
  let fixture: ComponentFixture<VisaStampingDocDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [VisaStampingDocDialogComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VisaStampingDocDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
