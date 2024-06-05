import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TestingAgencyComponent } from './testing-agency.component';

describe('TestingAgencyComponent', () => {
  let component: TestingAgencyComponent;
  let fixture: ComponentFixture<TestingAgencyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [TestingAgencyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TestingAgencyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
