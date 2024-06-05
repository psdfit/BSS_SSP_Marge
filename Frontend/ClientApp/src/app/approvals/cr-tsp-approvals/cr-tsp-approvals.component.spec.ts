import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TSPChangeRequestApprovalsComponent } from './cr-tsp-approvals.component';

describe('TSPChangeRequestApprovalsComponent', () => {
    let component: TSPChangeRequestApprovalsComponent;
    let fixture: ComponentFixture<TSPChangeRequestApprovalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [TSPChangeRequestApprovalsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(TSPChangeRequestApprovalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
