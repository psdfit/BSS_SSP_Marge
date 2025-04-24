import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GenerateMrnComponent } from './generate-mrn.component';

describe('GenerateMrnComponent', () => {
  let component: GenerateMrnComponent;
  let fixture: ComponentFixture<GenerateMrnComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GenerateMrnComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GenerateMrnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
