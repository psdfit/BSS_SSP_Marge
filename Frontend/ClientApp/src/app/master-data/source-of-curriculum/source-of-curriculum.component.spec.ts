import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SourceOfCurriculumComponent } from './source-of-curriculum.component';

describe('SourceOfCurriculumComponent', () => {
    let component: SourceOfCurriculumComponent;
    let fixture: ComponentFixture<SourceOfCurriculumComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [SourceOfCurriculumComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(SourceOfCurriculumComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
