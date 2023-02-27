import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { Product } from 'src/app/models/product';
import { ProductService } from 'src/app/services/product.service';
import { SnackbarService } from 'src/app/shared/snackbar.service';

@Component({
    selector: 'app-edit-product',
    templateUrl: './edit-product.component.html',
    styleUrls: ['./edit-product.component.css']
})
export class EditProductComponent implements OnInit {

    product$: Observable<Product> = of();

    constructor(
        private route: ActivatedRoute,
        private productService: ProductService,
        private router: Router,
        private snackbar: SnackbarService) { }

    ngOnInit(): void {
        this.product$ = of(new Product());
        this.route.params.subscribe(params => {
          const id = params['id'];
          if (!id) {
            return;
          }

          this.productService.get(id).subscribe(data => {
            this.product$ = of(data);
          });
        });
    }

    save(product: Product) {

        this.productService.edit(product).subscribe(res =>
            this.router.navigate(['/products'])
        );
    }

    cancel() {
        this.router.navigate(['/products']);
    }
}
