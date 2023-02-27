import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { LoginResponse } from "../models/login.response";
import { AppSettings } from "../shared/app.settings";

@Injectable({
    providedIn: 'root'
})
export class AuthService {

    constructor(private httpClient: HttpClient) { }

    public login(username: string, password: string) : Observable<LoginResponse>  {
        return this.httpClient.post<LoginResponse>(`${AppSettings.API_ENDPOINT}/Authenticate/login`, {username: username, password: password});
    }
}
