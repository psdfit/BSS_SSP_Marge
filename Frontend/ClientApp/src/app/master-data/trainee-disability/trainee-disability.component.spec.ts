import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TraineeDisabilityComponent } from './trainee-disability.component';

describe('TraineeDisabilityComponent', () => {
  let component: TraineeDisabilityComponent;
  let fixture: ComponentFixture<TraineeDisabilityComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TraineeDisabilityComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TraineeDisabilityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
