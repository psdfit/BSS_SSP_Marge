import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { DEOEmploymentVerificationDialogComponent } from '../deo-employment-verification-dialog/deo-employment-verification-dialog.component';

//import { DEOEmploymentVerificationDialogComponent } from './deo-employment-verification-dialog.component';

describe('DEOEmploymentVerificationDialogComponent', () => {
  let component: DEOEmploymentVerificationDialogComponent;
  let fixture: ComponentFixture<DEOEmploymentVerificationDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [DEOEmploymentVerificationDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DEOEmploymentVerificationDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
