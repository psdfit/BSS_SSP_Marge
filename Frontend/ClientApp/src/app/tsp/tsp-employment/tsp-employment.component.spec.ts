import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TSPEmpComponent } from './tsp-employment.component';

describe('PBTEComponent', () => {
    let component: TSPEmpComponent;
    let fixture: ComponentFixture<TSPEmpComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [TSPEmpComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(TSPEmpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
