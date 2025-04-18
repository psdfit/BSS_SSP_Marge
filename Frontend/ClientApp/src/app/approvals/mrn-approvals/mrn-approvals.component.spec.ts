import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MrnApprovalsComponent } from './mrn-approvals.component';

describe('MrnApprovalsComponent', () => {
  let component: MrnApprovalsComponent;
  let fixture: ComponentFixture<MrnApprovalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MrnApprovalsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MrnApprovalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
