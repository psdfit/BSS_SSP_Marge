import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CriteriaTemplateComponent } from './criteria-template.component';

describe('CriteriaTemplateComponent', () => {
  let component: CriteriaTemplateComponent;
  let fixture: ComponentFixture<CriteriaTemplateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CriteriaTemplateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CriteriaTemplateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
