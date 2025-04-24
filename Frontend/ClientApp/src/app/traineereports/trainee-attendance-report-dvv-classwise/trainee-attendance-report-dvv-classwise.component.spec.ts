import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TraineeEnrollmentComponent } from './trainee-enrollment.component';

describe('TraineeEnrollmentComponent', () => {
  let component: TraineeEnrollmentComponent;
  let fixture: ComponentFixture<TraineeEnrollmentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TraineeEnrollmentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TraineeEnrollmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
