import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DEOOJTEmploymentVerificationDialogComponent } from './deo-ojt-employment-verification-dialog.component';

describe('DEOOJTEmploymentVerificationDialogComponent', () => {
  let component: DEOOJTEmploymentVerificationDialogComponent;
  let fixture: ComponentFixture<DEOOJTEmploymentVerificationDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [DEOOJTEmploymentVerificationDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DEOOJTEmploymentVerificationDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
