import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OjtDeoVerificationComponent } from './ojt-deo-verification.component';

describe('OjtDeoVerificationComponent', () => {
  let component: OjtDeoVerificationComponent;
  let fixture: ComponentFixture<OjtDeoVerificationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OjtDeoVerificationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OjtDeoVerificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
