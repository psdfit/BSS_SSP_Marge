import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { GeoTaggingComponent } from './geo-tagging.component';

describe('GeoTaggingComponent', () => {
  let component: GeoTaggingComponent;
  let fixture: ComponentFixture<GeoTaggingComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GeoTaggingComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GeoTaggingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
