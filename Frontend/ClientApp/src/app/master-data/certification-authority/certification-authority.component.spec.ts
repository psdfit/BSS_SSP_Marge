import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CertificationAuthorityComponent } from './certification-authority.component';

describe('CertificationAuthorityComponent', () => {
  let component: CertificationAuthorityComponent;
  let fixture: ComponentFixture<CertificationAuthorityComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [CertificationAuthorityComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CertificationAuthorityComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
