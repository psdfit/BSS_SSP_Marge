import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { KAMComponent } from './KAM.component';

describe('KAMComponent', () => {
  let component: KAMComponent;
  let fixture: ComponentFixture<KAMComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [KAMComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(KAMComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
