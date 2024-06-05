import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PBTEComponent } from './pbte.component';

describe('PBTEComponent', () => {
    let component: PBTEComponent;
    let fixture: ComponentFixture<PBTEComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [PBTEComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(PBTEComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
