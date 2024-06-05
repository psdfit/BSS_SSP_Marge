import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TSPTraineeListComponent } from './tsp-trainee-list.component';

describe('TSPTraineeListComponent', () => {
  let component: TSPTraineeListComponent;
  let fixture: ComponentFixture<TSPTraineeListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [TSPTraineeListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TSPTraineeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
