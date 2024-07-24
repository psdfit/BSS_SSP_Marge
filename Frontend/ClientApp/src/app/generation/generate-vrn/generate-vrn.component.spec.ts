import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GenerateVrnComponent } from './generate-vrn.component';

describe('GenerateVrnComponent', () => {
  let component: GenerateVrnComponent;
  let fixture: ComponentFixture<GenerateVrnComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GenerateVrnComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GenerateVrnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
