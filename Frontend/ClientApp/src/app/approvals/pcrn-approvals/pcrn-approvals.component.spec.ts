import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PcrnApprovalsComponent } from './pcrn-approvals.component';

describe('PcrnApprovalsComponent', () => {
  let component: PcrnApprovalsComponent;
  let fixture: ComponentFixture<PcrnApprovalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PcrnApprovalsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PcrnApprovalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
