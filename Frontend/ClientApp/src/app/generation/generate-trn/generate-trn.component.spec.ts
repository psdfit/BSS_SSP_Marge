import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { GeneratePrnFinalComponent } from '../generate-prn-final/generate-prn-final.component';

//import { GeneratePrnFinalComponent } from './generate-prn-final.component';

describe('GeneratePrnFinalComponent', () => {
  let component: GeneratePrnFinalComponent;
  let fixture: ComponentFixture<GeneratePrnFinalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GeneratePrnFinalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GeneratePrnFinalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
