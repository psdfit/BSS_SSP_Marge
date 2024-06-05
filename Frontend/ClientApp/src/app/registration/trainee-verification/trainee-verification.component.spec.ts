import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { TraineeVarificationComponent } from './trainee-verification.component';


describe('TraineeVarificationComponent', () => {
  let component: TraineeVarificationComponent;
  let fixture: ComponentFixture<TraineeVarificationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TraineeVarificationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TraineeVarificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
