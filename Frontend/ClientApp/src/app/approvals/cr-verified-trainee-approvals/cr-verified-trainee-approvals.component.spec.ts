import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VerifiedTraineeChangeRequestApprovalsComponent } from './cr-verified-trainee-approvals.component';

describe('VerifiedTraineeChangeRequestApprovalsComponent', () => {
  let component: VerifiedTraineeChangeRequestApprovalsComponent;
  let fixture: ComponentFixture<VerifiedTraineeChangeRequestApprovalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [VerifiedTraineeChangeRequestApprovalsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VerifiedTraineeChangeRequestApprovalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
