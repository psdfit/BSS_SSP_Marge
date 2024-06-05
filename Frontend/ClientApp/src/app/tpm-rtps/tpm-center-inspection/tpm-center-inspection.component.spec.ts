import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TPMCenterInspectionComponent } from './tpm-center-inspection.component';

describe('TPMCenterInspectionComponent', () => {
    let component: TPMCenterInspectionComponent;
    let fixture: ComponentFixture<TPMCenterInspectionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [TPMCenterInspectionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(TPMCenterInspectionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
