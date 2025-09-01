import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GenerateOjtComponent } from './generate-ojt.component';

describe('GenerateOjtComponent', () => {
  let component: GenerateOjtComponent;
  let fixture: ComponentFixture<GenerateOjtComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GenerateOjtComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GenerateOjtComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
