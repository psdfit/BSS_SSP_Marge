import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FormalEmploymentVerificationComponent } from './formal-employment-verification.component';

describe('PurchaseOrderComponent', () => {
  let component: FormalEmploymentVerificationComponent;
  let fixture: ComponentFixture<FormalEmploymentVerificationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [FormalEmploymentVerificationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FormalEmploymentVerificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
