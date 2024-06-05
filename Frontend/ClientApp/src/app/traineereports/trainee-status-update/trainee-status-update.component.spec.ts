import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TraineeStatusUpdateComponent } from './trainee-status-update.component';

describe('TraineeStatusUpdateComponent', () => {
  let component: TraineeStatusUpdateComponent;
  let fixture: ComponentFixture<TraineeStatusUpdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TraineeStatusUpdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TraineeStatusUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
