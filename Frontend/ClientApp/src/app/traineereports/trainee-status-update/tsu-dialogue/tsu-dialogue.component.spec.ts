import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TsuDialogueComponent } from './tsu-dialogue.component';

describe('TsuDialogueComponent', () => {
  let component: TsuDialogueComponent;
  let fixture: ComponentFixture<TsuDialogueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TsuDialogueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TsuDialogueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
