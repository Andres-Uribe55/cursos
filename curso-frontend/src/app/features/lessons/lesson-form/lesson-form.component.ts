import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LessonService } from '../../../core/services/lesson.service';
import { Lesson, LessonCreate, LessonUpdate } from '../../../core/models/lesson.model';

@Component({
  selector: 'app-lesson-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './lesson-form.component.html',
  styleUrl: './lesson-form.component.css'
})
export class LessonFormComponent implements OnInit {
  @Input() courseId: string = '';
  @Input() lesson: Lesson | null = null;
  @Output() saved = new EventEmitter<void>();
  @Output() cancelled = new EventEmitter<void>();

  title = '';
  order = 1;
  loading = false;
  error = '';

  constructor(private lessonService: LessonService) { }

  ngOnInit(): void {
    if (this.lesson) {
      this.title = this.lesson.title;
      this.order = this.lesson.order;
    }
  }

  onSubmit(): void {
    if (!this.title.trim()) {
      this.error = 'El título es requerido';
      return;
    }

    if (this.order < 1) {
      this.error = 'El orden debe ser mayor a 0';
      return;
    }

    this.loading = true;
    this.error = '';

    if (this.lesson) {
      const data: LessonUpdate = {
        title: this.title,
        order: this.order
      };
      this.lessonService.update(this.lesson.id, data).subscribe({
        next: () => {
          this.saved.emit();
        },
        error: (err) => {
          this.error = err.error?.message || 'Error al actualizar la lección';
          this.loading = false;
        }
      });
    } else {
      const data: LessonCreate = {
        courseId: this.courseId,
        title: this.title,
        order: this.order
      };
      this.lessonService.create(data).subscribe({
        next: () => {
          this.saved.emit();
        },
        error: (err) => {
          this.error = err.error?.message || 'Error al crear la lección';
          this.loading = false;
        }
      });
    }
  }

  onCancel(): void {
    this.cancelled.emit();
  }
}