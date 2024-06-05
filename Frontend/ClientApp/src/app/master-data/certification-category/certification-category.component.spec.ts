import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CertificationCategoryComponent } from './certification-category.component';

describe('EducationTypeComponent', () => {
  let component: CertificationCategoryComponent;
  let fixture: ComponentFixture<CertificationCategoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [CertificationCategoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CertificationCategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
