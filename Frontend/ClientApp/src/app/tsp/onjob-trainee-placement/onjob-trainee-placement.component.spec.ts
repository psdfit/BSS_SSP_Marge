import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OnjobTraineePlacementComponent } from './onjob-trainee-placement.component';

describe('OnjobTraineePlacementComponent', () => {
  let component: OnjobTraineePlacementComponent;
  let fixture: ComponentFixture<OnjobTraineePlacementComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OnjobTraineePlacementComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OnjobTraineePlacementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
