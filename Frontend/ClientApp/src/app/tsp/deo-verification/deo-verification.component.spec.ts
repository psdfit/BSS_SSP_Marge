import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DeoVerificationComponent } from './deo-verification.component';

describe('DeoVerificationComponent', () => {
  let component: DeoVerificationComponent;
  let fixture: ComponentFixture<DeoVerificationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DeoVerificationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DeoVerificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
