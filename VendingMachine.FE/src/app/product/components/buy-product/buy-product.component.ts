import { Component, OnInit, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Observable, of } from 'rxjs';
import { BuyResponse } from 'src/app/models/buy.response';
import { TransactionService } from 'src/app/services/transaction.service';
import { SnackbarService } from 'src/app/shared/snackbar.service';
@Component({
    selector: 'app-buy-product',
    templateUrl: './buy-product.component.html',
    styleUrls: ['./buy-product.component.css']
})
export class BuyProductComponent implements OnInit {
    isBuySuccesfull: boolean = false;
    amountToBuy: number = 0;
    productId: string = '';
    productName: string = '';
    buyResponse$: Observable<BuyResponse> = of();

    constructor(public dialogRef: MatDialogRef<BuyProductComponent>,@Inject(MAT_DIALOG_DATA) public data: any, private transactionService: TransactionService, private snackbar: SnackbarService) {

        this.productId = data.productId;
        this.productName = data.productName;
     }

    ngOnInit(): void {
    }

    buy(){
        this.transactionService.buy(this.productId, this.amountToBuy).subscribe({
            next: (response: any) => {
                this.isBuySuccesfull = true;
                this.buyResponse$ = of(response);
            },
            error: (error)=>{
                if(error!=undefined && error.error != undefined && error.error.Message != undefined)
                {
                    this.snackbar.open(error.error.Message);
                }
                else{
                    this.snackbar.open("An error occurred!");
                }
            }
        });
    }

    closeModal() {
        this.dialogRef.close();
    }
}