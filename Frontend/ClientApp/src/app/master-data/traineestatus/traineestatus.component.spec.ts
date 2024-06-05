import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TraineeStatusComponent } from './traineestatus.component';

describe('TraineeStatusComponent', () => {
  let component: TraineeStatusComponent;
  let fixture: ComponentFixture<TraineeStatusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [TraineeStatusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TraineeStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
