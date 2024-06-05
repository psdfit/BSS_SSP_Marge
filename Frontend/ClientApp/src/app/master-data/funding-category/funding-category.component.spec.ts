import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FundingCategoryComponent } from './funding-category.component';

describe('FundingCategoryComponent', () => {
  let component: FundingCategoryComponent;
  let fixture: ComponentFixture<FundingCategoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [FundingCategoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FundingCategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
