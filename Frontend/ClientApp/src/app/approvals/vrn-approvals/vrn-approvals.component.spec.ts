import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VrnApprovalsComponent } from './vrn-approvals.component';

describe('VrnApprovalsComponent', () => {
  let component: VrnApprovalsComponent;
  let fixture: ComponentFixture<VrnApprovalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VrnApprovalsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VrnApprovalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
