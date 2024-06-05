import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TPMReportsComponent } from './tpm-reports.component';

describe('TPMReportsComponent', () => {
  let component: TPMReportsComponent;
  let fixture: ComponentFixture<TPMReportsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TPMReportsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TPMReportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
