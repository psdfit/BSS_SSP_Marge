import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { EmpVerificationComponent } from '../employment-verification/employment-verification.component';

//import { EmpVerificationComponent } from './employment-verification.component';

describe('PBTEComponent', () => {
  let component: EmpVerificationComponent;
  let fixture: ComponentFixture<EmpVerificationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [EmpVerificationComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmpVerificationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
