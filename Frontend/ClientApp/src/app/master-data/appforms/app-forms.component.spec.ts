import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AppFormsComponent } from './app-forms.component';

describe('AppFormsComponent', () => {
  let component: AppFormsComponent;
  let fixture: ComponentFixture<AppFormsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppFormsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppFormsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
