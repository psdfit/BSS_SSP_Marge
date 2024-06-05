import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClassChangeRequestApprovalsComponent } from './cr-class-approvals.component';

describe('ClassChangeRequestApprovalsComponent', () => {
    let component: ClassChangeRequestApprovalsComponent;
    let fixture: ComponentFixture<ClassChangeRequestApprovalsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [ClassChangeRequestApprovalsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(ClassChangeRequestApprovalsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
