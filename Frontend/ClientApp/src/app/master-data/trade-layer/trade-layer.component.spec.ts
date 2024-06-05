import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TradeLayerComponent } from './trade-layer.component';

describe('TradeLayerComponent', () => {
  let component: TradeLayerComponent;
  let fixture: ComponentFixture<TradeLayerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [TradeLayerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TradeLayerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
