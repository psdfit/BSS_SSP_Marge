import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InceptionReportComponent } from './inception-report.component';

describe('InceptionReportComponent', () => {
  let component: InceptionReportComponent;
  let fixture: ComponentFixture<InceptionReportComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [InceptionReportComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InceptionReportComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
