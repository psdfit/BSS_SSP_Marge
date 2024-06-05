import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { NotificationMappingComponent } from './notification-mapping.component';

describe('NotificationMappingComponent', () => {
  let component: NotificationMappingComponent;
  let fixture: ComponentFixture<NotificationMappingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ NotificationMappingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(NotificationMappingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
