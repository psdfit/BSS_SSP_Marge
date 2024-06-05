import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TraineeUpdationComponent } from './trainee-updation.component';

describe('TraineeUpdationComponent', () => {
  let component: TraineeUpdationComponent;
  let fixture: ComponentFixture<TraineeUpdationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TraineeUpdationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TraineeUpdationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
