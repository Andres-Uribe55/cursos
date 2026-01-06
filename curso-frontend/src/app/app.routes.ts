import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { adminGuard } from './core/guards/admin.guard';

export const routes: Routes = [
    { path: '', redirectTo: '/courses', pathMatch: 'full' },
    {
        path: 'login',
        loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent)
    },
    {
        path: 'register',
        loadComponent: () => import('./features/auth/register/register.component').then(m => m.RegisterComponent)
    },
    {
        path: 'profile',
        canActivate: [authGuard],
        loadComponent: () => import('./features/profile/profile.component').then(m => m.ProfileComponent)
    },
    {
        path: 'courses',
        canActivate: [authGuard],
        loadComponent: () => import('./features/courses/course-list/course-list.component').then(m => m.CourseListComponent)
    },
    {
        path: 'courses/new',
        canActivate: [authGuard, adminGuard],
        loadComponent: () => import('./features/courses/course-form/course-form.component').then(m => m.CourseFormComponent)
    },
    {
        path: 'courses/:id',
        canActivate: [authGuard],
        loadComponent: () => import('./features/courses/course-detail/course-detail.component').then(m => m.CourseDetailComponent)
    },
    {
        path: 'courses/:id/edit',
        canActivate: [authGuard, adminGuard],
        loadComponent: () => import('./features/courses/course-form/course-form.component').then(m => m.CourseFormComponent)
    },
    {
        path: 'students',
        canActivate: [authGuard, adminGuard],
        loadComponent: () => import('./features/students/student-list/student-list.component').then(m => m.StudentListComponent)
    },
    {
        path: 'students/new',
        canActivate: [authGuard, adminGuard],
        loadComponent: () => import('./features/students/student-form/student-form.component').then(m => m.StudentFormComponent)
    },
    {
        path: 'students/import',
        canActivate: [authGuard, adminGuard],
        loadComponent: () => import('./features/students/student-csv-upload/student-csv-upload.component').then(m => m.StudentCsvUploadComponent)
    },
    { path: '**', redirectTo: '/courses' }
];