import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DualEnrollmentCheckComponent } from './dual-enrollment-check.component';

describe('DualEnrollmentCheckComponent', () => {
    let component: DualEnrollmentCheckComponent;
    let fixture: ComponentFixture<DualEnrollmentCheckComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [DualEnrollmentCheckComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(DualEnrollmentCheckComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
