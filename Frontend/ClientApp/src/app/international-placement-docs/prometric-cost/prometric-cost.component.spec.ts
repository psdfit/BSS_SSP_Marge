import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PrometricCostComponent } from './prometric-cost.component';

describe('PrometricCostComponent', () => {
  let component: PrometricCostComponent;
  let fixture: ComponentFixture<PrometricCostComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [PrometricCostComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PrometricCostComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
