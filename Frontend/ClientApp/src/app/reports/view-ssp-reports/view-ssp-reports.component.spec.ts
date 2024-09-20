import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewSspReportsComponent } from './view-ssp-reports.component';

describe('ViewSspReportsComponent', () => {
  let component: ViewSspReportsComponent;
  let fixture: ComponentFixture<ViewSspReportsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewSspReportsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewSspReportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
