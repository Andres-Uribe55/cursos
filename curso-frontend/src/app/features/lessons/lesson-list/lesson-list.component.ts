import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LessonService } from '../../../core/services/lesson.service';
import { AuthService } from '../../../core/services/auth.service';
import { Lesson } from '../../../core/models/lesson.model';
import { LessonFormComponent } from '../lesson-form/lesson-form.component';

@Component({
  selector: 'app-lesson-list',
  standalone: true,
  imports: [CommonModule, LessonFormComponent],
  templateUrl: './lesson-list.component.html',
  styleUrl: './lesson-list.component.css'
})
export class LessonListComponent {
  @Input() courseId: string = '';
  @Input() lessons: Lesson[] = [];
  @Output() lessonsUpdated = new EventEmitter<void>();

  showForm = false;
  editingLesson: Lesson | null = null;

  constructor(
    private lessonService: LessonService,
    public authService: AuthService
  ) { }

  openCreateForm(): void {
    this.editingLesson = null;
    this.showForm = true;
  }

  openEditForm(lesson: Lesson): void {
    this.editingLesson = lesson;
    this.showForm = true;
  }

  closeForm(): void {
    this.showForm = false;
    this.editingLesson = null;
  }

  onLessonSaved(): void {
    this.closeForm();
    this.lessonsUpdated.emit();
  }

  deleteLesson(id: string): void {
    if (!confirm('¿Está seguro de eliminar esta lección?')) return;

    this.lessonService.delete(id).subscribe({
      next: () => {
        this.lessonsUpdated.emit();
      },
      error: (err) => {
        alert('Error al eliminar la lección');
      }
    });
  }

  moveUp(id: string): void {
    this.lessonService.moveUp(id).subscribe({
      next: () => {
        this.lessonsUpdated.emit();
      },
      error: (err) => {
        alert(err.error?.message || 'Error al reordenar');
      }
    });
  }

  moveDown(id: string): void {
    this.lessonService.moveDown(id).subscribe({
      next: () => {
        this.lessonsUpdated.emit();
      },
      error: (err) => {
        alert(err.error?.message || 'Error al reordenar');
      }
    });
  }

  canMoveUp(lesson: Lesson): boolean {
    return lesson.order > 1;
  }

  canMoveDown(lesson: Lesson): boolean {
    const maxOrder = Math.max(...this.lessons.map(l => l.order));
    return lesson.order < maxOrder;
  }
}