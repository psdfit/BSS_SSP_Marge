/* **** Aamer Rehman Malik *****/
import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DeletionApprovalsComponent } from './deletion-approvals.component';

describe('DeletionApprovalsComponent', () => {
  let component: DeletionApprovalsComponent;
  let fixture: ComponentFixture<DeletionApprovalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [DeletionApprovalsComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DeletionApprovalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
