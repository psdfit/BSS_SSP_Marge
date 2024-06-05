import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AssociationDetailComponent } from './association-detail.component';

describe('AssociationDetailComponent', () => {
  let component: AssociationDetailComponent;
  let fixture: ComponentFixture<AssociationDetailComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AssociationDetailComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AssociationDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
