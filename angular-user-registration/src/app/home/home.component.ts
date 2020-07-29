import { Component, OnInit } from '@angular/core';
import { TokenStorageService } from '../services/token-storage.service';
import { User } from '../modals/user';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  token:string;
  currentUser:any;
  username:string;

  constructor(private tokenStorage: TokenStorageService) { }
  
  ngOnInit(): void {        
  
        this.currentUser = this.tokenStorage.getUser();
        console.log('user is', this.currentUser);
        //this.username = this.currentUser.userName;
      }

  }


