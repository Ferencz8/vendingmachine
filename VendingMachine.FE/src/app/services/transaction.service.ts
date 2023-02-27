import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { BuyResponse } from "../models/buy.response";
import { AppSettings } from "../shared/app.settings";

@Injectable({
    providedIn: 'root'
})
export class TransactionService {

    constructor(private httpClient: HttpClient) { }

    public resetDeposit() {
        return this.httpClient.get(`${AppSettings.API_ENDPOINT}/Transaction/reset`);
    }

    public deposit(amount: number) {
        return this.httpClient.get(`${AppSettings.API_ENDPOINT}/Transaction/deposit?deposit=${amount}`);
    }

    public buy(productId: string, amount: number): Observable<BuyResponse>{
        return this.httpClient.post<BuyResponse>(`${AppSettings.API_ENDPOINT}/Transaction/buy`, {ProductId: productId, AmountOfProducts: amount});
    }
}
