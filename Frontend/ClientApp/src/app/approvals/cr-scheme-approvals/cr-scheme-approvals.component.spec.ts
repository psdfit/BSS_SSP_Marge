import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SchemeChangeRequestApprovalsComponent } from './cr-scheme-approvals.component';

describe('SchemeChangeRequestApprovalsComponent', () => {
    let component: SchemeChangeRequestApprovalsComponent;
    let fixture: ComponentFixture<SchemeChangeRequestApprovalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [SchemeChangeRequestApprovalsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(SchemeChangeRequestApprovalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
