import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ErrorLogTableComponent } from './error-log-table.component';

describe('ErrorLogTableComponent', () => {
  let component: ErrorLogTableComponent;
  let fixture: ComponentFixture<ErrorLogTableComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ErrorLogTableComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ErrorLogTableComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
