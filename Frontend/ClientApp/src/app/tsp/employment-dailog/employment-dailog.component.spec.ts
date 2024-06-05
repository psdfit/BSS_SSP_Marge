import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmploymentDailogComponent } from './employment-dailog.component';

describe('EmploymentDailogComponent', () => {
  let component: EmploymentDailogComponent;
  let fixture: ComponentFixture<EmploymentDailogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmploymentDailogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmploymentDailogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
