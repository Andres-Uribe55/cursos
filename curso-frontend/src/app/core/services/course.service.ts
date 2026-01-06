import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import {
    Course,
    CourseCreate,
    CourseUpdate,
    CourseSummary,
    CourseSearchParams,
    PaginatedResponse
} from '../models/course.model';

@Injectable({
    providedIn: 'root'
})
export class CourseService {
    private apiUrl = `${environment.apiUrl}/courses`;

    constructor(private http: HttpClient) { }

    search(params: CourseSearchParams): Observable<PaginatedResponse<Course>> {
        let httpParams = new HttpParams();

        if (params.q) httpParams = httpParams.set('q', params.q);
        if (params.status) httpParams = httpParams.set('status', params.status);
        if (params.page) httpParams = httpParams.set('page', params.page.toString());
        if (params.pageSize) httpParams = httpParams.set('pageSize', params.pageSize.toString());

        return this.http.get<PaginatedResponse<Course>>(`${this.apiUrl}/search`, { params: httpParams });
    }

    getById(id: string): Observable<Course> {
        return this.http.get<Course>(`${this.apiUrl}/${id}`);
    }

    getSummary(id: string): Observable<CourseSummary> {
        return this.http.get<CourseSummary>(`${this.apiUrl}/${id}/summary`);
    }

    create(data: CourseCreate): Observable<Course> {
        return this.http.post<Course>(this.apiUrl, data);
    }

    update(id: string, data: CourseUpdate): Observable<Course> {
        return this.http.put<Course>(`${this.apiUrl}/${id}`, data);
    }

    delete(id: string): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }

    publish(id: string): Observable<void> {
        return this.http.patch<void>(`${this.apiUrl}/${id}/publish`, {});
    }

    unpublish(id: string): Observable<void> {
        return this.http.patch<void>(`${this.apiUrl}/${id}/unpublish`, {});
    }
}