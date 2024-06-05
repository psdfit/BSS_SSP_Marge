import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseraReportComponent } from './coursera-report.component';

describe('CourseraReportComponent', () => {
  let component: CourseraReportComponent;
  let fixture: ComponentFixture<CourseraReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CourseraReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CourseraReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
