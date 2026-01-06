import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const adminGuard: CanActivateFn = (route, state) => {
    const authService = inject(AuthService);
    const router = inject(Router);

    if (authService.isAuthenticated() && authService.isAdmin()) {
        return true;
    }

    // Optional: Redirect to courses if authenticated but not admin
    if (authService.isAuthenticated()) {
        return router.parseUrl('/courses');
    }

    return router.parseUrl('/login');
};
