import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SrnDisbursementStatusComponent } from './srn-disbursement-status.component';

describe('SrnDisbursementStatusComponent', () => {
  let component: SrnDisbursementStatusComponent;
  let fixture: ComponentFixture<SrnDisbursementStatusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SrnDisbursementStatusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SrnDisbursementStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
