import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RTPApprovalsComponent } from './rtp-approvals.component';

describe('RTPApprovalsComponent', () => {
    let component: RTPApprovalsComponent;
    let fixture: ComponentFixture<RTPApprovalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [RTPApprovalsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(RTPApprovalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
