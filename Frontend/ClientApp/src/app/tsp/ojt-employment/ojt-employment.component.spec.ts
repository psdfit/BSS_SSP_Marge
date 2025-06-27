import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OJTEmpComponent } from './ojt-employment.component';

describe('PBTEComponent', () => {
    let component: OJTEmpComponent;
    let fixture: ComponentFixture<OJTEmpComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [OJTEmpComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(OJTEmpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
