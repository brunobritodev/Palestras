import { AuthConfig } from 'angular-oauth2-oidc';
import { environment } from '../../../environments/environment';

export const authConfig: AuthConfig = {
    issuer: environment.IssuerUri,
    clientId: 'meetup-angular',
    requireHttps: environment.RequireHttps,
    redirectUri: environment.Uri + "/login-callback",
    silentRefreshRedirectUri: environment.Uri + '/silent-refresh.html',
    scope: "openid profile email meetup-financeiro.leitura-cripto meetup-financeiro.leitura-dolar",
    silentRefreshTimeout: 5000, // For faster testing
    timeoutFactor: 0.25, // For faster testing
    sessionChecksEnabled: true,
    showDebugInformation: true, // Also requires enabling "Verbose" level in devtools
    clearHashAfterLogin: false, // https://github.com/manfredsteyer/angular-oauth2-oidc/issues/457#issuecomment-431807040
};
