export interface UserProfile {
    id: string;
    email: string;
    firstName: string;
    lastName: string;
    documentNumber?: string;
    age?: number;
    role: string;
}

export interface UpdateProfileRequest {
    firstName: string;
    lastName: string;
    documentNumber?: string;
    age?: number;
}

export interface ChangePasswordRequest {
    currentPassword: string;
    newPassword: string;
    confirmNewPassword: string;
}
