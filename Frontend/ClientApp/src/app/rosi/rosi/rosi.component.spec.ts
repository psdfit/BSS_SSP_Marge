import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ROSIComponent } from './rosi.component';

describe('ROSIComponent', () => {
    let component: ROSIComponent;
    let fixture: ComponentFixture<ROSIComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [ROSIComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(ROSIComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
