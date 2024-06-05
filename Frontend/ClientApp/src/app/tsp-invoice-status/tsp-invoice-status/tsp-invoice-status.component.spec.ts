import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TSPInvoiceStatusComponent } from './tsp-invoice-status.component';

describe('TSPInvoiceStatusComponent', () => {
  let component: TSPInvoiceStatusComponent;
  let fixture: ComponentFixture<TSPInvoiceStatusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [TSPInvoiceStatusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TSPInvoiceStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
