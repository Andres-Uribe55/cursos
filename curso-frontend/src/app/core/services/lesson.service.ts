import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Lesson, LessonCreate, LessonUpdate, ReorderLesson } from '../models/lesson.model';

@Injectable({
    providedIn: 'root'
})
export class LessonService {
    private apiUrl = `${environment.apiUrl}/lessons`;

    constructor(private http: HttpClient) { }

    getByCourse(courseId: string): Observable<Lesson[]> {
        return this.http.get<Lesson[]>(`${this.apiUrl}/course/${courseId}`);
    }

    getById(id: string): Observable<Lesson> {
        return this.http.get<Lesson>(`${this.apiUrl}/${id}`);
    }

    create(data: LessonCreate): Observable<Lesson> {
        return this.http.post<Lesson>(this.apiUrl, data);
    }

    update(id: string, data: LessonUpdate): Observable<Lesson> {
        return this.http.put<Lesson>(`${this.apiUrl}/${id}`, data);
    }

    delete(id: string): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }

    reorder(data: ReorderLesson): Observable<void> {
        return this.http.patch<void>(`${this.apiUrl}/reorder`, data);
    }

    moveUp(id: string): Observable<void> {
        return this.http.patch<void>(`${this.apiUrl}/${id}/move-up`, {});
    }

    moveDown(id: string): Observable<void> {
        return this.http.patch<void>(`${this.apiUrl}/${id}/move-down`, {});
    }
}