import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TspRegistrationReportComponent } from './tsp-registration-report.component';

describe('TspRegistrationReportComponent', () => {
  let component: TspRegistrationReportComponent;
  let fixture: ComponentFixture<TspRegistrationReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TspRegistrationReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TspRegistrationReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
