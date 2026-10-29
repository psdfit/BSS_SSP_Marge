import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TspInvoiceSubmissionComponent } from './tsp-invoice-submission.component';

describe('TspInvoiceSubmissionComponent', () => {
  let component: TspInvoiceSubmissionComponent;
  let fixture: ComponentFixture<TspInvoiceSubmissionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TspInvoiceSubmissionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TspInvoiceSubmissionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
