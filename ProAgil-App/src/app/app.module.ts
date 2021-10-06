import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ModalModule } from 'ngx-bootstrap/modal';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDatepickerConfig, BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ToastrModule } from 'ngx-toastr';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { NgxMaskModule } from 'ngx-mask'
import { NgxCurrencyModule } from 'ngx-currency';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { EventosComponent } from './eventos/eventos.component';
import { EventoEditComponent } from './eventos/eventoEdit/eventoEdit.component';
import { NavComponent } from './nav/nav.component';
import { DateTimeFormatPipePipe } from './_helps/DateTimeFormatPipe.pipe';

import { EventoService } from './_services/evento.service';
import { PalestrantesComponent } from './palestrantes/palestrantes.component';
import { ContatosComponent } from './contatos/contatos.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { TituloComponent } from './_shared/titulo/titulo.component';
import { UserComponent } from './user/user.component';
import { LoginComponent } from './user/login/login.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { AuthInterceptor } from './auth/auth.interceptor';


@NgModule({
  declarations: [						
    AppComponent,
      EventosComponent,
      EventoEditComponent,
      PalestrantesComponent,
      ContatosComponent,
      DashboardComponent,
      TituloComponent,
      NavComponent,
      DateTimeFormatPipePipe,
      UserComponent,
      LoginComponent,
      RegistrationComponent
   ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    BsDatepickerModule.forRoot(),
    ToastrModule.forRoot({
      timeOut: 10000,
      preventDuplicates: true
    }),
    BsDropdownModule.forRoot(),
    TooltipModule.forRoot(),
    ModalModule.forRoot(),
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    TabsModule.forRoot(),
    NgxMaskModule.forRoot(),
    NgxCurrencyModule
  ],
  providers: [
    EventoService,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

