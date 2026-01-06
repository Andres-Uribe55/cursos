export interface LoginRequest {
    email: string;
    password: string;
}

export interface RegisterRequest {
    email: string;
    password: string;
    confirmPassword: string;
}

export interface AuthResponse {
    token: string;
    refreshToken: string;
    email: string;
    role: string;
}

export interface RefreshTokenRequest {
    token: string;
    refreshToken: string;
}