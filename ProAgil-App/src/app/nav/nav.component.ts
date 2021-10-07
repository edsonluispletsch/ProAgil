import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../_services/auth.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  constructor(private toastr: ToastrService,
    private authService: AuthService,
    public router: Router) { }

  ngOnInit() {
  }

  showMenu() {
    return this.router.url !== '/user/login';
  }

  loggedIn() {
    const logado = this.authService.loggedIn();
    const token = localStorage.getItem('token');
    
    if ((!logado) && (token)) {
      localStorage.removeItem('token');
      this.router.navigate(['/user/login'])
    }
    return logado;
  }

  userName() {
    return sessionStorage.getItem('username');
//    return this.authService.decodeToken?.unique_name;
  }

  entrar() {
    this.router.navigate(['/user/login'])
  } 

  logout() {
    localStorage.removeItem('token');
    this.toastr.show('Log out');
    this.router.navigate(['/user/login'])
  }  
}
