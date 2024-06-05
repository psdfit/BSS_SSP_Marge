import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InflationRateComponent } from './year-wise-inflation-rate.component';

describe('EducationTypeComponent', () => {
    let component: InflationRateComponent;
    let fixture: ComponentFixture<InflationRateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [InflationRateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(InflationRateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
