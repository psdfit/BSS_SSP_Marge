import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GenerateOtrnComponent } from './generate-otrn.component';

describe('GenerateOtrnComponent', () => {
  let component: GenerateOtrnComponent;
  let fixture: ComponentFixture<GenerateOtrnComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GenerateOtrnComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GenerateOtrnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
