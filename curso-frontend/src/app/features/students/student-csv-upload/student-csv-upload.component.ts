import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router, RouterLink } from '@angular/router';
import { StudentService } from '../../../core/services/student.service';

@Component({
  selector: 'app-student-csv-upload',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './student-csv-upload.component.html',
  styleUrl: './student-csv-upload.component.css'
})
export class StudentCsvUploadComponent {
  selectedFile: File | null = null;
  loading = false;
  error = '';
  success = '';

  constructor(
    private studentService: StudentService,
    private router: Router
  ) { }

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0] ?? null;
    this.error = '';
  }

  upload(): void {
    if (!this.selectedFile) return;

    this.loading = true;
    this.error = '';
    this.success = '';

    this.studentService.uploadCsv(this.selectedFile).subscribe({
      next: () => {
        this.success = 'Archivo procesado correctamente';
        this.loading = false;
        setTimeout(() => this.router.navigate(['/students']), 2000);
      },
      error: (err) => {
        this.error = 'Error al procesar el archivo';
        this.loading = false;
      }
    });
  }
}
