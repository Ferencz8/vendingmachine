import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from '../login/login.component';
import { AddProductComponent } from '../product/components/add-product/add-product.component';
import { EditProductComponent } from '../product/components/edit-product/edit-product.component';
import { ProductListComponent } from '../product/components/product-list/product-list.component';
import { HomeComponent } from './home.component';

const routes: Routes = [
    {
        path: 'home', component: HomeComponent, children: [
        ]
    },
    { path: 'products', component: ProductListComponent },
    { path: 'addProduct', component: AddProductComponent },
    { path: 'editProduct/:id', component: EditProductComponent },
    { path: 'login', component: LoginComponent },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class HomeRoutingModule { }
