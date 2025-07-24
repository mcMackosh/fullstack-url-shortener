import { makeAutoObservable } from 'mobx';

interface User {
  username: string;
  role: string;
  id: string;
}

class UserStore {
  user: User | null = null;
  isAuth: boolean = false;
  id: string | null = null;

  constructor() {
    makeAutoObservable(this);
    const savedUser = localStorage.getItem('user');
    const savedToken = localStorage.getItem('token');

    if (savedToken && savedUser) {
      this.user = JSON.parse(savedUser);
      this.isAuth = true;
    }
  }

  login(user: User, token: string) {
    this.user = user;
    this.isAuth = true;
    this.id = user.id;

    localStorage.setItem('user', JSON.stringify(user));
    localStorage.setItem('token', token);
  }

  logout() {
    this.user = null;
    this.isAuth = false;

    localStorage.removeItem('user');
    localStorage.removeItem('token');
  }
}

export const userStore = new UserStore();
