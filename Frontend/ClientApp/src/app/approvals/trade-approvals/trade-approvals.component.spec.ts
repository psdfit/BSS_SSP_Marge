import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TradeApprovalsComponent } from './trade-approvals.component';

describe('TradeApprovalsComponent', () => {
    let component: TradeApprovalsComponent;
    let fixture: ComponentFixture<TradeApprovalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [TradeApprovalsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(TradeApprovalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
