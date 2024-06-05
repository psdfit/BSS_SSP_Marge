import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TelephonicComponent } from './telephonic.component';

describe('TelephonicComponent', () => {
  let component: TelephonicComponent;
  let fixture: ComponentFixture<TelephonicComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TelephonicComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TelephonicComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
