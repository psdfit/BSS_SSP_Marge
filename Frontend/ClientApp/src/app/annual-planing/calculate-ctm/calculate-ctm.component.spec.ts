import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CalculateCtmComponent } from './calculate-ctm.component';

describe('CalculateCtmComponent', () => {
  let component: CalculateCtmComponent;
  let fixture: ComponentFixture<CalculateCtmComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CalculateCtmComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CalculateCtmComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
