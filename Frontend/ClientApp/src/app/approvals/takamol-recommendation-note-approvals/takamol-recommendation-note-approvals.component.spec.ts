import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OtrnApprovalsComponent } from './otrn-approvals.component';

describe('OtrnApprovalsComponent', () => {
  let component: OtrnApprovalsComponent;
  let fixture: ComponentFixture<OtrnApprovalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OtrnApprovalsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OtrnApprovalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
