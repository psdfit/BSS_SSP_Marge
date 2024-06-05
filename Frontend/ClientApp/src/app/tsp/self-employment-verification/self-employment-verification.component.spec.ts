import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SelfEmploymentVerificationComponent } from './self-employment-verification.component';

describe('PurchaseOrderComponent', () => {
  let component: SelfEmploymentVerificationComponent;
  let fixture: ComponentFixture<SelfEmploymentVerificationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [SelfEmploymentVerificationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SelfEmploymentVerificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
