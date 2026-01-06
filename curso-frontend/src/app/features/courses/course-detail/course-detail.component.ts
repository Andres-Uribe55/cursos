import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CourseService } from '../../../core/services/course.service';
import { LessonService } from '../../../core/services/lesson.service';
import { AuthService } from '../../../core/services/auth.service';
import { CourseSummary, CourseStatus } from '../../../core/models/course.model';
import { Lesson } from '../../../core/models/lesson.model';
import { LessonListComponent } from '../../lessons/lesson-list/lesson-list.component';

@Component({
  selector: 'app-course-detail',
  standalone: true,
  imports: [CommonModule, RouterLink, LessonListComponent],
  templateUrl: './course-detail.component.html',
  styleUrl: './course-detail.component.css'
})
export class CourseDetailComponent implements OnInit {
  courseId: string = '';
  courseSummary: CourseSummary | null = null;
  lessons: Lesson[] = [];
  loading = false;
  error = '';

  CourseStatus = CourseStatus;

  constructor(
    private route: ActivatedRoute,
    private courseService: CourseService,
    private lessonService: LessonService,
    public authService: AuthService
  ) { }

  ngOnInit(): void {
    this.courseId = this.route.snapshot.paramMap.get('id') || '';
    if (this.courseId) {
      this.loadCourseDetails();
      this.loadLessons();
    }
  }

  loadCourseDetails(): void {
    this.loading = true;
    this.courseService.getSummary(this.courseId).subscribe({
      next: (summary) => {
        this.courseSummary = summary;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Error al cargar el curso';
        this.loading = false;
      }
    });
  }

  loadLessons(): void {
    this.lessonService.getByCourse(this.courseId).subscribe({
      next: (lessons) => {
        this.lessons = lessons;
      },
      error: (err) => {
        console.error('Error al cargar lecciones', err);
      }
    });
  }

  onLessonsUpdated(): void {
    this.loadLessons();
    this.loadCourseDetails();
  }

  publish(): void {
    if (confirm('¿Estás seguro de publicar este curso?')) {
      this.courseService.publish(this.courseId).subscribe({
        next: () => this.loadCourseDetails(),
        error: () => alert('Error al publicar curso')
      });
    }
  }

  unpublish(): void {
    if (confirm('¿Estás seguro de despublicar este curso?')) {
      this.courseService.unpublish(this.courseId).subscribe({
        next: () => this.loadCourseDetails(),
        error: () => alert('Error al despublicar curso')
      });
    }
  }
}