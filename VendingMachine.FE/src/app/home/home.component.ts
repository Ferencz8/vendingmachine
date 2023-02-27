import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TransactionService } from '../services/transaction.service';
import { SnackbarService } from '../shared/snackbar.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {

  amountToDeposit: number = 0;
  constructor(private transactionService: TransactionService, private router: Router, private snackbar: SnackbarService) { }


  resetDeposit(){
    this.transactionService.resetDeposit().subscribe(res => {
      this.snackbar.open('Deposit was reseted to 0 succesfully');
    });
  }

  deposit(){
    this.transactionService.deposit(this.amountToDeposit).subscribe(res => {
      this.snackbar.open(`Deposit was increased with ${this.amountToDeposit} succesfully`);
    });
  }

  viewProducts(){
    this.router.navigate(['/products']);
  }
}
