import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable()
export class SnackbarService{

  constructor(private _snackBar: MatSnackBar) {}

  public open(message: string, action = 'success', durationInSeconds = 5) {
    let duration: number = durationInSeconds * 1000;
    this._snackBar.open(message, action, { duration });
  }
}
