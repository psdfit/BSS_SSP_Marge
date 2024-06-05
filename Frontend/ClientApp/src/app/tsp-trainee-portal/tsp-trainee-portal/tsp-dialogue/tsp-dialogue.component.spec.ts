import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TspDialogueComponent } from './tsp-dialogue.component';

describe('TsuDialogueComponent', () => {
  let component: TspDialogueComponent;
  let fixture: ComponentFixture<TspDialogueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TspDialogueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TspDialogueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
