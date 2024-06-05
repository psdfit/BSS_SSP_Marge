import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TspStatusUpdateComponent } from './tsp-status-update.component';

describe('TspStatusUpdateComponent', () => {
  let component: TspStatusUpdateComponent;
  let fixture: ComponentFixture<TspStatusUpdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TspStatusUpdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TspStatusUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
