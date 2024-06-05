import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProgramCategoryComponent } from './program-category.component';

describe('ProgramCategoryComponent', () => {
  let component: ProgramCategoryComponent;
  let fixture: ComponentFixture<ProgramCategoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ProgramCategoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProgramCategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
