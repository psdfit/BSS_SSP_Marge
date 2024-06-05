import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TraineePSPAssignmentComponent } from './trainee-psp-assignment.component';

describe('TraineePSPAssignmentComponent', () => {
  let component: TraineePSPAssignmentComponent;
  let fixture: ComponentFixture<TraineePSPAssignmentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [TraineePSPAssignmentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TraineePSPAssignmentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
