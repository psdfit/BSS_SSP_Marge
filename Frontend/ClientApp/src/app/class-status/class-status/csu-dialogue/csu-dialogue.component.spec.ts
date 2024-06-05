import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CsuDialogueComponent } from './csu-dialogue.component';

describe('CsuDialogueComponent', () => {
  let component: CsuDialogueComponent;
  let fixture: ComponentFixture<CsuDialogueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CsuDialogueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CsuDialogueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
