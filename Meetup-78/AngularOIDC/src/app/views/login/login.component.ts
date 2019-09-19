import { Component, OnDestroy, OnInit } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { AuthService } from '../../core/auth/auth.service';
import { Subscription } from 'rxjs';
import { Router } from '@angular/router';

@Component({
    selector: 'app-dashboard',
    templateUrl: 'login.component.html'
})
export class LoginComponent implements OnInit, OnDestroy {
    private stream: Subscription;

    constructor(
        private authService: AuthService,
        private router: Router) {

    }

    public ngOnInit() {
        this.stream = this.authService.canActivateProtectedRoutes$.subscribe(yes => {
            if (yes)
                this.router.navigate(['/home']);
            else
                this.authService.login('/login-callback');
        });
    }

    public ngOnDestroy() {
        this.stream.unsubscribe();
    }

    public login() { this.authService.login('/login-callback'); }

}
