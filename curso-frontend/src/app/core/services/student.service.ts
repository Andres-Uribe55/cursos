import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { Student, StudentCreate } from '../models/student.model';
import { PaginatedResponse } from '../models/course.model';

@Injectable({
    providedIn: 'root'
})
export class StudentService {
    private apiUrl = `${environment.apiUrl}/students`;

    constructor(private http: HttpClient) { }

    search(q?: string, page: number = 1, pageSize: number = 10): Observable<PaginatedResponse<Student>> {
        let params = new HttpParams()
            .set('page', page.toString())
            .set('pageSize', pageSize.toString());

        if (q) params = params.set('q', q);

        return this.http.get<PaginatedResponse<Student>>(`${this.apiUrl}/search`, { params });
    }

    getById(id: string): Observable<Student> {
        return this.http.get<Student>(`${this.apiUrl}/${id}`);
    }

    create(data: StudentCreate): Observable<Student> {
        return this.http.post<Student>(this.apiUrl, data);
    }

    delete(id: string): Observable<void> {
        return this.http.delete<void>(`${this.apiUrl}/${id}`);
    }

    uploadCsv(file: File): Observable<any> {
        const formData = new FormData();
        formData.append('file', file);
        return this.http.post(`${this.apiUrl}/import-csv`, formData);
    }
}
