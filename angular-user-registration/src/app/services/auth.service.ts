import { Injectable } from '@angular/core';
import { User } from '../modals/user';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import *  as config from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
export class AuthService {
  
    constructor(private http: HttpClient) {
        
    }   

    login(username, password) {
        return this.http.post<any>
                  (`${config.environment.AuthAPI}/users/signin`, 
                    {
                      username, 
                      password 
                    },
                    httpOptions
                  );
    }
    register(user:User):Observable<any> {
      return this.http.post<any>
              (`${config.environment.AuthAPI}/users/signup`, 
                {
                    username: user.username,
                    email: user.email,
                    password: user.password
                },
                httpOptions
              );
    }    
}
