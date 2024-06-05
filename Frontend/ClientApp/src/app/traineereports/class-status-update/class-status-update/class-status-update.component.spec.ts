import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClassStatusUpdateComponent } from './class-status-update.component';

describe('ClassStatusUpdateComponent', () => {
  let component: ClassStatusUpdateComponent;
  let fixture: ComponentFixture<ClassStatusUpdateComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClassStatusUpdateComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClassStatusUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
