import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PoApprovalsComponent } from './po-approvals.component';

describe('PoApprovalsComponent', () => {
  let component: PoApprovalsComponent;
  let fixture: ComponentFixture<PoApprovalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PoApprovalsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PoApprovalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
