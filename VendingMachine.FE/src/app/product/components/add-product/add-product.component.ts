import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable, of } from 'rxjs';
import { Product } from 'src/app/models/product';
import { ProductService } from 'src/app/services/product.service';
import { SnackbarService } from 'src/app/shared/snackbar.service';

@Component({
    selector: 'app-add-product',
    templateUrl: './add-product.component.html',
    styleUrls: ['./add-product.component.css']
})
export class AddProductComponent implements OnInit {

    product$: Observable<Product> = of();

    constructor(
        private route: ActivatedRoute,
        private productService: ProductService,
        private router: Router,
        private snackbar: SnackbarService) { }

    ngOnInit(): void {
        this.product$ = of(new Product());
    }

    save(product: Product) {

        this.productService.add(product).subscribe(res =>
            this.router.navigate(['/products'])
        );
    }

    cancel() {
        this.router.navigate(['/products']);
    }
}
