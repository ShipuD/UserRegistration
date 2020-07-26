import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { TokenStorageService } from '../services/token-storage.service';
import { FormsModule }   from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  isLoggedIn = false;
  isLoginFailed = false;
  errorMessage = '';
  form:any = {};

  constructor(private authService: AuthService, 
      private tokenStorage: TokenStorageService) 
    {
      
    }
  ngOnInit() {
      if(this.tokenStorage.getToken())
      this.isLoggedIn = true;
    }

  onSubmit() {
    this.authService.login(this.form).subscribe (
      data => {
        this.tokenStorage.saveToken(data.token);
        this.isLoggedIn = true;
        this.isLoginFailed = false;
        this.reloadPage();
      },
      err=> {
        this.errorMessage = err.error.message;
        this.isLoginFailed = true;        
      }
      );
  }

  reloadPage(){  
    window.location.reload();
  }

}
