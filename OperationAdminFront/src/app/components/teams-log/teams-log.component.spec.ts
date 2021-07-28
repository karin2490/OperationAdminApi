import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TeamsLogComponent } from './teams-log.component';

describe('TeamsLogComponent', () => {
  let component: TeamsLogComponent;
  let fixture: ComponentFixture<TeamsLogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TeamsLogComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TeamsLogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
