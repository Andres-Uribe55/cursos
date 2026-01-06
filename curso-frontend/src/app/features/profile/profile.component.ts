import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProfileService } from '../../core/services/profile.service';
import { UserProfile, UpdateProfileRequest, ChangePasswordRequest } from '../../core/models/profile.model';

@Component({
    selector: 'app-profile',
    standalone: true,
    imports: [CommonModule, FormsModule],
    templateUrl: './profile.component.html',
    styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
    activeTab: 'info' | 'security' = 'info';
    profile: UserProfile | null = null;
    loading = false;
    successMessage = '';
    errorMessage = '';

    profileData: UpdateProfileRequest = {
        firstName: '',
        lastName: '',
        age: undefined,
        documentNumber: ''
    };

    passwordData: ChangePasswordRequest = {
        currentPassword: '',
        newPassword: '',
        confirmNewPassword: ''
    };

    constructor(private profileService: ProfileService) { }

    ngOnInit(): void {
        this.loadProfile();
    }

    loadProfile(): void {
        this.loading = true;
        this.profileService.getProfile().subscribe({
            next: (data) => {
                this.profile = data;
                this.profileData = {
                    firstName: data.firstName,
                    lastName: data.lastName,
                    age: data.age,
                    documentNumber: data.documentNumber
                };
                this.loading = false;
            },
            error: (err) => {
                this.errorMessage = 'Error al cargar perfil';
                this.loading = false;
            }
        });
    }

    updateInfo(): void {
        this.loading = true;
        this.successMessage = '';
        this.errorMessage = '';

        this.profileService.updateProfile(this.profileData).subscribe({
            next: (updatedProfile) => {
                this.profile = updatedProfile;
                this.successMessage = 'Información actualizada correctamente';
                this.loading = false;
                setTimeout(() => this.successMessage = '', 3000);
            },
            error: (err) => {
                this.errorMessage = err.error?.message || 'Error al actualizar información';
                this.loading = false;
            }
        });
    }

    changePassword(): void {
        if (this.passwordData.newPassword !== this.passwordData.confirmNewPassword) {
            this.errorMessage = 'Las contraseñas nuevas no coinciden';
            return;
        }

        this.loading = true;
        this.successMessage = '';
        this.errorMessage = '';

        this.profileService.changePassword(this.passwordData).subscribe({
            next: () => {
                this.successMessage = 'Contraseña actualizada correctamente';
                this.passwordData = { currentPassword: '', newPassword: '', confirmNewPassword: '' };
                this.loading = false;
                setTimeout(() => this.successMessage = '', 3000);
            },
            error: (err) => {
                this.errorMessage = err.error?.message || 'Error al cambiar contraseña';
                this.loading = false;
            }
        });
    }

    get userInitials(): string {
        if (!this.profile) return '';
        return (this.profile.firstName?.[0] || '') + (this.profile.lastName?.[0] || '');
    }
}
