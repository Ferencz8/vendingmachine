import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { Product } from 'src/app/models/product';
import { ProductService } from 'src/app/services/product.service';
import { SnackbarService } from 'src/app/shared/snackbar.service';
import { MatDialog, MatDialogConfig, MatDialogRef } from '@angular/material/dialog';
import { BuyProductComponent } from '../buy-product/buy-product.component';
@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: ['./product-list.component.css']
})
export class ProductListComponent implements OnInit, AfterViewInit {
  dialogConfig = new MatDialogConfig();
  modalDialog: MatDialogRef<BuyProductComponent, any> | undefined;

  displayedColumns: string[] = ['name', 'amountAvailable', 'cost', 'actions'];

  dataSource$: Observable<Product[]> = of();

  constructor(private productService: ProductService, private snackbar: SnackbarService, private router: Router, private matDialog: MatDialog) { }
  ngAfterViewInit(): void {
    document.onclick = (args: any): void => {
      if (args.target.tagName === 'BODY') {
        this.modalDialog?.close()
      }
    }
  }

  ngOnInit(): void {

    this.productService.getAll().subscribe((res: any) => { this.dataSource$ = of(res); });
    //this.dataSource$ = of(this.mydata);
  }

  buy(id: any, name: any): void {

      this.dialogConfig.id = "projects-modal-component";
      this.dialogConfig.height = "500px";
      this.dialogConfig.width = "650px";
      this.dialogConfig.data = {productId: id, productName: name}
      this.modalDialog = this.matDialog.open(BuyProductComponent, this.dialogConfig);
  }

  edit(id: any): void {
    this.router.navigate([`/editProduct/${id}`]);
  }

  remove(id: any): void {
    this.router.navigate([`/removeProduct/${id}`]);
  }
} 
