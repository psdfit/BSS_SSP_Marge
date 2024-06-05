import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProcessStatusUpdateComponent } from './process-status-update.component';

describe('ProcessStatusUpdateComponent', () => {
  let component: ProcessStatusUpdateComponent;
  let fixture: ComponentFixture<ProcessStatusUpdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProcessStatusUpdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProcessStatusUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
