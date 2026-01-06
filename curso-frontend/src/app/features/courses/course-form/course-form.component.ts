import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { CourseService } from '../../../core/services/course.service';
import { CourseCreate, CourseUpdate } from '../../../core/models/course.model';

@Component({
  selector: 'app-course-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './course-form.component.html',
  styleUrl: './course-form.component.css'
})
export class CourseFormComponent implements OnInit {
  courseId: string | null = null;
  title = '';
  description = '';
  resourceUrl = '';
  loading = false;
  error = '';
  isEditMode = false;

  constructor(
    private courseService: CourseService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.courseId = this.route.snapshot.paramMap.get('id');
    if (this.courseId) {
      this.isEditMode = true;
      this.loadCourse();
    }
  }

  loadCourse(): void {
    if (!this.courseId) return;

    this.courseService.getById(this.courseId).subscribe({
      next: (course) => {
        this.title = course.title;
        this.description = course.description || '';
        this.resourceUrl = course.resourceUrl || '';
      },
      error: (err) => {
        this.error = 'Error al cargar el curso';
      }
    });
  }

  onSubmit(): void {
    if (!this.title.trim()) {
      this.error = 'El título es requerido';
      return;
    }

    this.loading = true;
    this.error = '';

    if (this.isEditMode && this.courseId) {
      const data: CourseUpdate = {
        title: this.title,
        description: this.description,
        resourceUrl: this.resourceUrl
      };
      this.courseService.update(this.courseId, data).subscribe({
        next: () => {
          this.router.navigate(['/courses']);
        },
        error: (err) => {
          this.error = err.error?.message || 'Error al actualizar el curso';
          this.loading = false;
        }
      });
    } else {
      const data: CourseCreate = {
        title: this.title,
        description: this.description,
        resourceUrl: this.resourceUrl
      };
      this.courseService.create(data).subscribe({
        next: () => {
          this.router.navigate(['/courses']);
        },
        error: (err) => {
          this.error = err.error?.message || 'Error al crear el curso';
          this.loading = false;
        }
      });
    }
  }
}