import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CourseraSrnComponent } from './coursera-srn.component';

describe('CourseraSrnComponent', () => {
  let component: CourseraSrnComponent;
  let fixture: ComponentFixture<CourseraSrnComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CourseraSrnComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CourseraSrnComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
