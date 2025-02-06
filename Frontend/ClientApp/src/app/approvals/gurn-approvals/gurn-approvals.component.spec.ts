import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GurnApprovalsComponent } from './gurn-approvals.component';

describe('GrnApprovalsComponent', () => {
  let component: GurnApprovalsComponent;
  let fixture: ComponentFixture<GurnApprovalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GurnApprovalsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GurnApprovalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
