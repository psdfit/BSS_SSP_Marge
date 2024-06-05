import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { TSRDialogComponent } from './tsr-dialog.component';

describe('SrnEditDialogComponent', () => {
  let component: TSRDialogComponent;
  let fixture: ComponentFixture<TSRDialogComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [TSRDialogComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TSRDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
