import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TrnApprovalsComponent } from './trn-approvals.component';

describe('TrnApprovalsComponent', () => {
  let component: TrnApprovalsComponent;
  let fixture: ComponentFixture<TrnApprovalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TrnApprovalsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TrnApprovalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
