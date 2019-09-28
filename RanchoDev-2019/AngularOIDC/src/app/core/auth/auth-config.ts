import { AuthConfig } from 'angular-oauth2-oidc';
import { environment } from '../../../environments/environment';

export const authConfig: AuthConfig = {
    issuer: environment.IssuerUri,
    clientId: 'is4-show-angular',
    requireHttps: environment.RequireHttps,
    redirectUri: environment.Uri + "/login-callback",
    silentRefreshRedirectUri: environment.Uri + '/silent-refresh.html',
    scope: "openid profile email is4-show-financeiro.leitura",
  
};
