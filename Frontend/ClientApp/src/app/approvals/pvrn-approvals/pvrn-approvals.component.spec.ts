import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PvrnApprovalsComponent } from './pvrn-approvals.component';

describe('PvrnApprovalsComponent', () => {
  let component: PvrnApprovalsComponent;
  let fixture: ComponentFixture<PvrnApprovalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PvrnApprovalsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PvrnApprovalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
