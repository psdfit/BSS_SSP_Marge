import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TradePlanComponent } from './trade-plan.component';

describe('TradePlanComponent', () => {
  let component: TradePlanComponent;
  let fixture: ComponentFixture<TradePlanComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TradePlanComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TradePlanComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
