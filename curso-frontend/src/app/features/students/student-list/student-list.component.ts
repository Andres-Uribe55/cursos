import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { StudentService } from '../../../core/services/student.service';
import { Student } from '../../../core/models/student.model';

@Component({
  selector: 'app-student-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './student-list.component.html',
  styleUrl: './student-list.component.css'
})
export class StudentListComponent implements OnInit {
  students: Student[] = [];
  loading = false;
  searchQuery = '';
  page = 1;
  pageSize = 10;
  totalItems = 0;

  constructor(private studentService: StudentService) { }

  ngOnInit(): void {
    this.loadStudents();
  }

  loadStudents(): void {
    this.loading = true;
    this.studentService.search(this.searchQuery, this.page, this.pageSize)
      .subscribe({
        next: (response) => {
          this.students = response.items;
          this.totalItems = response.totalCount;
          this.loading = false;
        },
        error: (err) => {
          console.error('Error loading students', err);
          this.loading = false;
        }
      });
  }

  onSearch(): void {
    this.page = 1;
    this.loadStudents();
  }

  changePage(newPage: number): void {
    this.page = newPage;
    this.loadStudents();
  }

  deleteStudent(id: string): void {
    if (confirm('¿Estás seguro de eliminar este estudiante?')) {
      this.studentService.delete(id).subscribe({
        next: () => {
          this.loadStudents();
        },
        error: (err) => alert('Error al eliminar estudiante')
      });
    }
  }
}
