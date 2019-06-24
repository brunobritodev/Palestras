import { OAuthModuleConfig } from 'angular-oauth2-oidc';
import { environment } from '../../../environments/environment';

export const authModuleConfig: OAuthModuleConfig = {
    resourceServer: {
        allowedUrls: [environment.ResourceServer],
        sendAccessToken: true
    }
};
