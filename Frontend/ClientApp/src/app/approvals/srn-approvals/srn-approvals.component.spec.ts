import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SrnApprovalsComponent } from './srn-approvals.component';

describe('SrnApprovalsComponent', () => {
  let component: SrnApprovalsComponent;
  let fixture: ComponentFixture<SrnApprovalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SrnApprovalsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SrnApprovalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
