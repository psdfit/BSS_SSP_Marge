import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgramReviewComponent } from './program-review.component';

describe('ProgramReviewComponent', () => {
  let component: ProgramReviewComponent;
  let fixture: ComponentFixture<ProgramReviewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProgramReviewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgramReviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
