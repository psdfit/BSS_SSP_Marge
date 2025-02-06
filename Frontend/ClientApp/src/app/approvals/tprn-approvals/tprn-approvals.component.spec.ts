import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TprnApprovalsComponent } from './tprn-approvals.component';

describe('TprnApprovalsComponent', () => {
  let component: TprnApprovalsComponent;
  let fixture: ComponentFixture<TprnApprovalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TprnApprovalsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TprnApprovalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
