import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StudentCsvUploadComponent } from './student-csv-upload.component';

describe('StudentCsvUploadComponent', () => {
  let component: StudentCsvUploadComponent;
  let fixture: ComponentFixture<StudentCsvUploadComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [StudentCsvUploadComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(StudentCsvUploadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
