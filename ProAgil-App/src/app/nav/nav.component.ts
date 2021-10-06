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

  loggedIn() {
    return this.authService.loggedIn();
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
