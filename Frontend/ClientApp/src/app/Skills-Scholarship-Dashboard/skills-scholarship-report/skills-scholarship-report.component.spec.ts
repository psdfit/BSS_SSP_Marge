import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SkillsScholarshipReportComponent } from './skills-scholarship-report.component';

describe('SkillsScholarshipReportComponent', () => {
  let component: SkillsScholarshipReportComponent;
  let fixture: ComponentFixture<SkillsScholarshipReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SkillsScholarshipReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SkillsScholarshipReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
