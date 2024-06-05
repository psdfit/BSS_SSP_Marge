import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PSPEmpComponent } from './psp-employment.component';

describe('PBTEComponent', () => {
    let component: PSPEmpComponent;
    let fixture: ComponentFixture<PSPEmpComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [PSPEmpComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(PSPEmpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
