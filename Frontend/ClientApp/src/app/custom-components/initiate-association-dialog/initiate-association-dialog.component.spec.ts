import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InitiateAssociationDialogComponent } from './initiate-association-dialog.component';

describe('InitiateAssociationDialogComponent', () => {
  let component: InitiateAssociationDialogComponent;
  let fixture: ComponentFixture<InitiateAssociationDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InitiateAssociationDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InitiateAssociationDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
