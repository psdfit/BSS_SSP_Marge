import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TSPNTPsComponent } from './tsp-ntps.component';

describe('TSPNTPsComponent', () => {
    let component: TSPNTPsComponent;
    let fixture: ComponentFixture<TSPNTPsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [TSPNTPsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
      fixture = TestBed.createComponent(TSPNTPsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
