import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClassStatusDialougeComponent } from './class-status-dialouge.component';

describe('ClassStatusDialougeComponent', () => {
  let component: ClassStatusDialougeComponent;
  let fixture: ComponentFixture<ClassStatusDialougeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClassStatusDialougeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClassStatusDialougeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
