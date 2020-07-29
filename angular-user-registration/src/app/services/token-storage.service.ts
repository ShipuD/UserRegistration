import { Injectable } from '@angular/core';
import { User } from '../modals/user';
const JWT_TOKEN_KEY = 'jwt-auth-token';
const USER_KEY = 'jwt-auth-user';

@Injectable({
  providedIn: 'root'
})
export class TokenStorageService {

  constructor() { }
  
  signOut() {
    window.sessionStorage.clear();
  }

  public saveToken(token: string) {
    window.sessionStorage.removeItem(JWT_TOKEN_KEY);
    window.sessionStorage.setItem(JWT_TOKEN_KEY, token);
  }

  public getToken(): string {
    return sessionStorage.getItem(JWT_TOKEN_KEY);
  }

  public saveUser(user:User) {
    window.sessionStorage.removeItem(USER_KEY);
    window.sessionStorage.setItem(USER_KEY, JSON.stringify(user));
  }

  public getUser() {
    return JSON.parse(sessionStorage.getItem(USER_KEY));
  }
}
