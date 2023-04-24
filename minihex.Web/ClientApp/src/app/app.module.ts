import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { BoardComponent } from './board/board.component';
import { BoardService } from './board/board.service';
import { CommunicationService } from './communication/communication.service';
import { UserSettingsComponent } from './user-settings/user-settings.component';
import { UserSettingsService } from './user-settings/user-settings.service';
import { InfoBarComponent } from './info-bar/info-bar.component';
import { ErrorInterceptor } from './error-interceptor/error-interceptor.service';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    BoardComponent,
    UserSettingsComponent,
    InfoBarComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
    ])
  ],
  providers: [BoardService, CommunicationService, UserSettingsService,
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true }],
  bootstrap: [AppComponent]
})
export class AppModule { }
