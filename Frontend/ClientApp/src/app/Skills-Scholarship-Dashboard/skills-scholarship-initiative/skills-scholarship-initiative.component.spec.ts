import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SkillsScholarshipComponent } from './skills-scholarship-initiative.component';

describe('SkillsScholarshipComponent', () => {
  let component: SkillsScholarshipComponent;
  let fixture: ComponentFixture<SkillsScholarshipComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SkillsScholarshipComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SkillsScholarshipComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
