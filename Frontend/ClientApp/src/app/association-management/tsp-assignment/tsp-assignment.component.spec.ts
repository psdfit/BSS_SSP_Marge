import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TspAssignmentComponent } from './tsp-assignment.component';

describe('TspAssignmentComponent', () => {
  let component: TspAssignmentComponent;
  let fixture: ComponentFixture<TspAssignmentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TspAssignmentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TspAssignmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
