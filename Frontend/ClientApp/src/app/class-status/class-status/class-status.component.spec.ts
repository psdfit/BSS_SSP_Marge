import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClassStatusComponent } from './class-status.component';

describe('ClassStatusComponent', () => {
  let component: ClassStatusComponent;
  let fixture: ComponentFixture<ClassStatusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClassStatusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClassStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
