import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomFinancialYearComponent } from './custom-financial-year.component';

describe('CustomFinancialYearComponent', () => {
  let component: CustomFinancialYearComponent;
  let fixture: ComponentFixture<CustomFinancialYearComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CustomFinancialYearComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CustomFinancialYearComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
