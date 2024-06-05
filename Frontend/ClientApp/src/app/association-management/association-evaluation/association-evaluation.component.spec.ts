import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociationEvaluationComponent } from './association-evaluation.component';

describe('AssociationEvaluationComponent', () => {
  let component: AssociationEvaluationComponent;
  let fixture: ComponentFixture<AssociationEvaluationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociationEvaluationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociationEvaluationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
