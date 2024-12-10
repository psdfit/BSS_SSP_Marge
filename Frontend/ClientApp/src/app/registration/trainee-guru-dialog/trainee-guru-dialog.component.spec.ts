import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TraineeGuruDialogComponent } from './trainee-guru-dialog.component';

describe('TraineeGuruDialogComponent', () => {
  let component: TraineeGuruDialogComponent;
  let fixture: ComponentFixture<TraineeGuruDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TraineeGuruDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TraineeGuruDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
