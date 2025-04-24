import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GeneratePvrnComponent } from './generate-pvrn.component';

describe('GeneratePvrnComponent', () => {
  let component: GeneratePvrnComponent;
  let fixture: ComponentFixture<GeneratePvrnComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GeneratePvrnComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GeneratePvrnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
