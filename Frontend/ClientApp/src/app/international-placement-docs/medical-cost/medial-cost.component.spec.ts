import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MedicalCostComponent } from './medial-cost.component';

describe('TraineeAttendanceComponent', () => {
  let component: MedicalCostComponent;
  let fixture: ComponentFixture<MedicalCostComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MedicalCostComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MedicalCostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
