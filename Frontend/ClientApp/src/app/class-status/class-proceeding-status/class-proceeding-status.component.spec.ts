import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClassProceedingStatusComponent } from './class-proceeding-status.component';

describe('ClassProceedingStatusComponent', () => {
  let component: ClassProceedingStatusComponent;
  let fixture: ComponentFixture<ClassProceedingStatusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClassProceedingStatusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClassProceedingStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
