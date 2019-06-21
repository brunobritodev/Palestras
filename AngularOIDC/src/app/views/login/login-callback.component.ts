import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router } from "@angular/router";
import { AuthService } from '../../core/auth/auth.service';
import { Subscription } from 'rxjs';

@Component({
    template: ''
})
export class LoginCallbackComponent implements OnInit, OnDestroy {
    stream: Subscription;
    constructor(private authService: AuthService, private router: Router) {
    }

    ngOnInit() {
        this.stream = this.authService.canActivateProtectedRoutes$.subscribe(yes => {
            if (yes)
                this.router.navigate(["/dashboard"]);

        });

    }
    public ngOnDestroy() {
        this.stream.unsubscribe();
    }
}
