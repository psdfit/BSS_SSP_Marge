import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TSPComponent } from '../tsp/tsp.component';

describe('TSPComponent', () => {
  let component: TSPComponent;
  let fixture: ComponentFixture<TSPComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TSPComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TSPComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
