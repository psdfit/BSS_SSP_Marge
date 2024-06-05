import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AcademicDisciplineComponent } from './academic-discipline.component';

describe('AcademicDisciplineComponent', () => {
    let component: AcademicDisciplineComponent;
    let fixture: ComponentFixture<AcademicDisciplineComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [AcademicDisciplineComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(AcademicDisciplineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
