import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgramTypeComponent } from './programtype.component';

describe('ProgramTypeComponent', () => {
  let component: ProgramTypeComponent;
  let fixture: ComponentFixture<ProgramTypeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ProgramTypeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgramTypeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
