import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgramInitiateComponent } from './program-initiate.component';

describe('ProgramInitiateComponent', () => {
  let component: ProgramInitiateComponent;
  let fixture: ComponentFixture<ProgramInitiateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProgramInitiateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgramInitiateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
