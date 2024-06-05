import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { StipendStatusComponent } from './stipendstatus.component';

describe('StipendStatusComponent', () => {
  let component: StipendStatusComponent;
  let fixture: ComponentFixture<StipendStatusComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [StipendStatusComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(StipendStatusComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
