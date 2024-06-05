import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ResultStatusComponent } from './resultstatus.component';

describe('ResultStatusComponent', () => {
  let component: ResultStatusComponent;
  let fixture: ComponentFixture<ResultStatusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ResultStatusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ResultStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
