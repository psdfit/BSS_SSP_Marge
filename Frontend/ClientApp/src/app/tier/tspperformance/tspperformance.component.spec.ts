import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TSPPerformanceComponent } from './tspperformance.component';

describe('TSPPerformanceComponent', () => {
  let component: TSPPerformanceComponent;
  let fixture: ComponentFixture<TSPPerformanceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TSPPerformanceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TSPPerformanceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
