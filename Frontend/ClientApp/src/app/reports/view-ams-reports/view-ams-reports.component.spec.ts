import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewAmsReportsComponent } from './view-ams-reports.component';

describe('ViewAmsReportsComponent', () => {
  let component: ViewAmsReportsComponent;
  let fixture: ComponentFixture<ViewAmsReportsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewAmsReportsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewAmsReportsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
