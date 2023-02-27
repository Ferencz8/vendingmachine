import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { SharedMaterialModule } from '../shared.module';
import { SnackbarService } from '../shared/snackbar.service';
import { ProductListComponent } from './components/product-list/product-list.component';
import { AddProductComponent } from './components/add-product/add-product.component';
import { EditProductComponent } from './components/edit-product/edit-product.component';
import { BuyProductComponent } from './components/buy-product/buy-product.component';
import { MatDialogModule } from '@angular/material/dialog';


@NgModule({
  declarations: [
    ProductListComponent,
    AddProductComponent,
    EditProductComponent,
    BuyProductComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    SharedMaterialModule,
    RouterModule,
    MatDialogModule
  ],
  exports: [ProductListComponent, AddProductComponent, EditProductComponent
  ],
  providers: [
    SnackbarService,
    BuyProductComponent
  ],
})
export class ProductModule { }
