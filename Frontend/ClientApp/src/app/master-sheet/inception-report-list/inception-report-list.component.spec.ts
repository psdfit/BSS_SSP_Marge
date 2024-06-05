import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InceptionReportListComponent } from './inception-report-list.component';

describe('InceptionReportListComponent', () => {
  let component: InceptionReportListComponent;
  let fixture: ComponentFixture<InceptionReportListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [InceptionReportListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InceptionReportListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
