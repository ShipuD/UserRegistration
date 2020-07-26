import { Injectable } from '@angular/core';
import { User } from '../modals/user';
import { BehaviorSubject, Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import *  as config from '../../environments/environment';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
@Injectable({
  providedIn: 'root'
})
export class AuthService {
  
    constructor(private http: HttpClient) {
        
    }   

    login(user:User) {
        return this.http.post<any>
                  (`${config.environment.AuthAPI}/users/authenticate`, 
                    {
                      username:user.username, 
                      password:user.password
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
