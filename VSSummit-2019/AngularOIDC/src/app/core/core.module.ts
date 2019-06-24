import { NgModule, Optional, SkipSelf, ModuleWithProviders } from "@angular/core";

import { throwIfAlreadyLoaded } from "./module-import-guard";
import { AuthGuardWithForcedLogin } from "./auth/auth-guard-with-forced-login.service";
import { AuthService } from "./auth/auth.service";
import { OAuthModule, AuthConfig, OAuthModuleConfig, ValidationHandler, JwksValidationHandler, OAuthStorage } from "angular-oauth2-oidc";
import { HttpClientModule } from "@angular/common/http";
import { authModuleConfig } from "./auth/auth-module-config";
import { AuthGuard } from "./auth/auth-guard.service";
import { authConfig } from "./auth/auth-config";

export function storageFactory(): OAuthStorage {
    return localStorage;
}


@NgModule({
    imports: [
        HttpClientModule,
        OAuthModule.forRoot(),
    ],
    providers: [
        AuthService,
        AuthGuard,
        AuthGuardWithForcedLogin,
    ],
    declarations: [
    ],
    exports: [
    ]
})
export class CoreModule {
    static forRoot(): ModuleWithProviders {
        return {
            ngModule: CoreModule,
            providers: [
                { provide: AuthConfig, useValue: authConfig },
                { provide: OAuthModuleConfig, useValue: authModuleConfig },
                { provide: ValidationHandler, useClass: JwksValidationHandler },
                { provide: OAuthStorage, useFactory: storageFactory },
            ]
        };
    }

    constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
        throwIfAlreadyLoaded(parentModule, "CoreModule");
    }
}
