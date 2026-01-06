import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { UserProfile, UpdateProfileRequest, ChangePasswordRequest } from '../models/profile.model';

@Injectable({
    providedIn: 'root'
})
export class ProfileService {
    private apiUrl = `${environment.apiUrl}/profile`;

    constructor(private http: HttpClient) { }

    getProfile(): Observable<UserProfile> {
        return this.http.get<UserProfile>(this.apiUrl);
    }

    updateProfile(data: UpdateProfileRequest): Observable<UserProfile> {
        return this.http.put<UserProfile>(this.apiUrl, data);
    }

    changePassword(data: ChangePasswordRequest): Observable<any> {
        return this.http.post(`${this.apiUrl}/change-password`, data);
    }
}
