import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociationPaymentComponent } from './association-payment.component';

describe('AssociationPaymentComponent', () => {
  let component: AssociationPaymentComponent;
  let fixture: ComponentFixture<AssociationPaymentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociationPaymentComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociationPaymentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
