import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProcessScheduleComponent } from './process-schedule.component';

describe('ProcessScheduleComponent', () => {
  let component: ProcessScheduleComponent;
  let fixture: ComponentFixture<ProcessScheduleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProcessScheduleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProcessScheduleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
