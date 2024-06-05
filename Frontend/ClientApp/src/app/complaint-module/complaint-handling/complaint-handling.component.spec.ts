import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ComplaintHandlingComponent } from './complaint-handling.component';

describe('ComplaintHandlingComponent', () => {
  let component: ComplaintHandlingComponent;
  let fixture: ComponentFixture<ComplaintHandlingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ComplaintHandlingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ComplaintHandlingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
