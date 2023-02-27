import { Change } from "./change";
import { Product } from "./product";

export class BuyResponse{
    change: Change = new Change();

    totalSpent: number = 0;

    countOfPurchasedProducts: number = 0;

    purchasedProduct: Product = new Product();
}