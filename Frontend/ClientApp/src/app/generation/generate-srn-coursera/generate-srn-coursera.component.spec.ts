import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GenerateSrnCourseraComponent } from './generate-srn-coursera.component';

describe('GenerateSrnCourseraComponent', () => {
  let component: GenerateSrnCourseraComponent;
  let fixture: ComponentFixture<GenerateSrnCourseraComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GenerateSrnCourseraComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GenerateSrnCourseraComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
