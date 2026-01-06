import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';
import { CourseService } from '../../../core/services/course.service';
import { Course, CourseStatus, CourseSearchParams } from '../../../core/models/course.model';

@Component({
  selector: 'app-course-list',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './course-list.component.html',
  styleUrl: './course-list.component.css'
})
export class CourseListComponent implements OnInit {
  courses: Course[] = [];
  searchQuery = '';
  selectedStatus: CourseStatus | '' = '';
  currentPage = 1;
  pageSize = 10;
  totalCount = 0;
  loading = false;
  error = '';

  CourseStatus = CourseStatus;

  constructor(
    private courseService: CourseService,
    public authService: AuthService
  ) { }

  ngOnInit(): void {
    this.loadCourses();
  }

  loadCourses(): void {
    this.loading = true;
    this.error = '';

    const params: CourseSearchParams = {
      page: this.currentPage,
      pageSize: this.pageSize
    };

    if (this.searchQuery) params.q = this.searchQuery;
    if (this.selectedStatus) params.status = this.selectedStatus as CourseStatus;

    this.courseService.search(params).subscribe({
      next: (response) => {
        this.courses = response.items;
        this.totalCount = response.totalCount;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar cursos';
        this.loading = false;
      }
    });
  }

  onSearch(): void {
    this.currentPage = 1;
    this.loadCourses();
  }

  onStatusChange(): void {
    this.currentPage = 1;
    this.loadCourses();
  }

  deleteCourse(id: string): void {
    if (!confirm('¿Está seguro de eliminar este curso?')) return;

    this.courseService.delete(id).subscribe({
      next: () => {
        this.loadCourses();
      },
      error: (err) => {
        alert('Error al eliminar el curso');
      }
    });
  }

  publishCourse(id: string): void {
    this.courseService.publish(id).subscribe({
      next: () => {
        this.loadCourses();
      },
      error: (err) => {
        alert(err.error?.message || 'Error al publicar el curso');
      }
    });
  }

  unpublishCourse(id: string): void {
    this.courseService.unpublish(id).subscribe({
      next: () => {
        this.loadCourses();
      },
      error: (err) => {
        alert('Error al despublicar el curso');
      }
    });
  }

  nextPage(): void {
    if (this.currentPage * this.pageSize < this.totalCount) {
      this.currentPage++;
      this.loadCourses();
    }
  }

  prevPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadCourses();
    }
  }

  get totalPages(): number {
    return Math.ceil(this.totalCount / this.pageSize);
  }
}