import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TPMRTPsComponent } from './tpm-rtps.component';

describe('TPMRTPsComponent', () => {
    let component: TPMRTPsComponent;
    let fixture: ComponentFixture<TPMRTPsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [TPMRTPsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(TPMRTPsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
