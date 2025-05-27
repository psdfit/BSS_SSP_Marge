import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GeneratePcrnComponent } from './generate-pcrn.component';

describe('GeneratePcrnComponent', () => {
  let component: GeneratePcrnComponent;
  let fixture: ComponentFixture<GeneratePcrnComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GeneratePcrnComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GeneratePcrnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
