import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PSPTraineeListComponent } from './psp-trainees-list.component';

describe('PSPTraineeListComponent', () => {
  let component: PSPTraineeListComponent;
  let fixture: ComponentFixture<PSPTraineeListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [PSPTraineeListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PSPTraineeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
