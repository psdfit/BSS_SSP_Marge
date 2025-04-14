import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { VisaStampingComponent } from './visa-stamping.component';

describe('TraineeEnrollmentComponent', () => {
  let component: VisaStampingComponent;
  let fixture: ComponentFixture<VisaStampingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ VisaStampingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(VisaStampingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
