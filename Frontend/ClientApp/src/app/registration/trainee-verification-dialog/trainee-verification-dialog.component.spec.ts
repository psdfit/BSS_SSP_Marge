import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TraineeVerificationDialogComponent } from './trainee-verification-dialog.component';

describe('TraineeVarificationDialogComponent', () => {
  let component: TraineeVerificationDialogComponent;
  let fixture: ComponentFixture<TraineeVerificationDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [TraineeVerificationDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TraineeVerificationDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
