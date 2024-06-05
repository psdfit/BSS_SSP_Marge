import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TraineeChangeRequestApprovalsComponent } from './cr-trainee-approvals.component';

describe('TraineeChangeRequestApprovalsComponent', () => {
    let component: TraineeChangeRequestApprovalsComponent;
    let fixture: ComponentFixture<TraineeChangeRequestApprovalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [TraineeChangeRequestApprovalsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(TraineeChangeRequestApprovalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
