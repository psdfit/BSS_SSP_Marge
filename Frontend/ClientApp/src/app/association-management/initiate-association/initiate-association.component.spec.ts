import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InitiateAssociationComponent } from './initiate-association.component';

describe('InitiateAssociationComponent', () => {
  let component: InitiateAssociationComponent;
  let fixture: ComponentFixture<InitiateAssociationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InitiateAssociationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InitiateAssociationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
