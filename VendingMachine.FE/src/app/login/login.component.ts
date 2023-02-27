import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  username: string = '';
  password: string = '';
  constructor(private authenticationService: AuthService, private router: Router) { }

  ngOnInit(): void {
  }

  login(): void{
    this.authenticationService.login(this.username, this.password).subscribe(authResult => {
      
        localStorage.setItem('id_token', authResult.token);
        localStorage.setItem("expires_at",authResult.expiration);
        
        this.router.navigate([`/home`]);
    });
  }
}
