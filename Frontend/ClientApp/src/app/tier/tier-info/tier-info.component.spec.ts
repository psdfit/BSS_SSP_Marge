import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TierInfoComponent } from './tier-info.component';

describe('TierInfoComponent', () => {
  let component: TierInfoComponent;
  let fixture: ComponentFixture<TierInfoComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TierInfoComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TierInfoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
