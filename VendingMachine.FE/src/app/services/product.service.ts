import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Product } from "../models/product";
import { AppSettings } from "../shared/app.settings";

@Injectable({
    providedIn: 'root'
})
export class ProductService {

    constructor(private httpClient: HttpClient) { }

    public getAll() : Observable<Product[]>{
        return this.httpClient.get<Product[]>(`${AppSettings.API_ENDPOINT}/Product/getall`);
    }

    public get(id: string) : Observable<Product>{
        return this.httpClient.get<Product>(`${AppSettings.API_ENDPOINT}/Product/get?id=${id}`);
    }

    public add(product: Product) {
        return this.httpClient.post(`${AppSettings.API_ENDPOINT}/Product/add`, product);
    }

    public edit(product: Product) {
        return this.httpClient.put(`${AppSettings.API_ENDPOINT}/Product/update`, product);
    }
}
