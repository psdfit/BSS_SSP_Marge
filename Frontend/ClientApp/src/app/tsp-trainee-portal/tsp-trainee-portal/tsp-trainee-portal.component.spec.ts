import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TspTraineePortalComponent } from './tsp-trainee-portal.component';

describe('TspTraineePortalComponent', () => {
  let component: TspTraineePortalComponent;
  let fixture: ComponentFixture<TspTraineePortalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TspTraineePortalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TspTraineePortalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
