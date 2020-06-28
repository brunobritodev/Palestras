import { Injectable } from '@angular/core';

import { HttpClient, HttpHeaders } from '../../../../node_modules/@angular/common/http';
import { Observable } from '../../../../node_modules/rxjs';
import { environment } from '../../../environments/environment';
import { Moedas } from '../../shared/models/moedas.model';

@Injectable({
    providedIn: 'root'
})
export class CotacaoService {

    private defaultHeader: HttpHeaders;
    constructor(private http: HttpClient) {
        this.defaultHeader = new HttpHeaders().set('Accept', 'application/json').set('Authorization', 'Bearer ' + JSON.parse(localStorage.getItem("jwtLogin")).token);

    }

    public obterCotacoes(): Observable<Moedas[]> {
        const url = "cotacao/cripto-moedas";

        return this.http.get<Moedas[]>(environment.API_URL + url);
    }
}
