import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject, tap } from 'rxjs';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';
import { LoginRequest, RegisterRequest, AuthResponse } from '../models/auth.model';

@Injectable({
    providedIn: 'root'
})
export class AuthService {
    private apiUrl = `${environment.apiUrl}/auth`;
    private tokenKey = 'auth_token';
    private refreshTokenKey = 'refresh_token';
    private roleKey = 'auth_role';
    private isAuthenticatedSubject = new BehaviorSubject<boolean>(this.hasToken());
    private userRoleSubject = new BehaviorSubject<string | null>(this.getRole());

    isAuthenticated$ = this.isAuthenticatedSubject.asObservable();
    userRole$ = this.userRoleSubject.asObservable();

    constructor(private http: HttpClient, private router: Router) { }

    register(data: RegisterRequest): Observable<AuthResponse> {
        return this.http.post<AuthResponse>(`${environment.apiUrl}/students/register`, data).pipe(
            tap(response => this.handleAuthResponse(response))
        );
    }

    login(data: LoginRequest, role: 'Admin' | 'Student' = 'Student'): Observable<AuthResponse> {
        const url = role === 'Admin'
            ? `${environment.apiUrl}/auth/login`
            : `${environment.apiUrl}/students/login`;

        return this.http.post<AuthResponse>(url, data).pipe(
            tap(response => this.handleAuthResponse(response))
        );
    }

    refreshToken(): Observable<AuthResponse> {
        const token = this.getToken();
        const refreshToken = this.getRefreshToken();

        if (!token || !refreshToken) {
            this.logout();
            throw new Error('No tokens available');
        }

        return this.http.post<AuthResponse>(`${this.apiUrl}/refresh-token`, { token, refreshToken }).pipe(
            tap(response => this.handleAuthResponse(response))
        );
    }

    logout(): void {
        localStorage.removeItem(this.tokenKey);
        localStorage.removeItem(this.refreshTokenKey);
        localStorage.removeItem(this.roleKey);
        this.isAuthenticatedSubject.next(false);
        this.userRoleSubject.next(null);
        this.router.navigate(['/login']);
    }

    private handleAuthResponse(response: AuthResponse): void {
        this.setToken(response.token);
        this.setRefreshToken(response.refreshToken);
        this.setRole(response.role);
        this.isAuthenticatedSubject.next(true);
        this.userRoleSubject.next(response.role);
    }

    getToken(): string | null {
        return localStorage.getItem(this.tokenKey);
    }

    private setToken(token: string): void {
        localStorage.setItem(this.tokenKey, token);
    }

    getRefreshToken(): string | null {
        return localStorage.getItem(this.refreshTokenKey);
    }

    private setRefreshToken(token: string): void {
        localStorage.setItem(this.refreshTokenKey, token);
    }

    getRole(): string | null {
        return localStorage.getItem(this.roleKey);
    }

    private setRole(role: string): void {
        localStorage.setItem(this.roleKey, role);
    }

    private hasToken(): boolean {
        return !!this.getToken();
    }

    isAuthenticated(): boolean {
        return this.hasToken();
    }

    isAdmin(): boolean {
        return this.getRole() === 'Admin';
    }

    isStudent(): boolean {
        return this.getRole() === 'Student';
    }
}