import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { ClassMonthviewComponent } from '../class-monthview/class-monthview.component';



describe('ClassMonthviewComponent', () => {
  let component: ClassMonthviewComponent;
  let fixture: ComponentFixture<ClassMonthviewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClassMonthviewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClassMonthviewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
