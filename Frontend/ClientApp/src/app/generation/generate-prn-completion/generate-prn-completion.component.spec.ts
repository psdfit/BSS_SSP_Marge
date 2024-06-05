import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GeneratePrnCompletionComponent } from './generate-prn-completion.component';

describe('GeneratePrnCompletionComponent', () => {
  let component: GeneratePrnCompletionComponent;
  let fixture: ComponentFixture<GeneratePrnCompletionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GeneratePrnCompletionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GeneratePrnCompletionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
