import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GenerateGuruRecommendationNoteComponent } from './generate-guru-recommendation-note.component';

describe('GeneratePrnCompletionComponent', () => {
  let component: GenerateGuruRecommendationNoteComponent;
  let fixture: ComponentFixture<GenerateGuruRecommendationNoteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [GenerateGuruRecommendationNoteComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GenerateGuruRecommendationNoteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
