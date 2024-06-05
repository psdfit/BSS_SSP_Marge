import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { FundingSourceComponent } from './funding-source.component';

describe('FundingSourceComponent', () => {
  let component: FundingSourceComponent;
  let fixture: ComponentFixture<FundingSourceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [FundingSourceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(FundingSourceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
