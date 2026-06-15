import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GenerateInternetNoteComponent } from './generate-internet-note.component';

describe('GenerateInternetNoteComponent', () => {
  let component: GenerateInternetNoteComponent;
  let fixture: ComponentFixture<GenerateInternetNoteComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GenerateInternetNoteComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GenerateInternetNoteComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
