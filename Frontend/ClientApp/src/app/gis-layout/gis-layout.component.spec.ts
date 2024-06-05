import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GisLayoutComponent } from './gis-layout.component';

describe('GisLayoutComponent', () => {
  let component: GisLayoutComponent;
  let fixture: ComponentFixture<GisLayoutComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GisLayoutComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GisLayoutComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
