import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { UIService } from 'src/business/services/shared/ui.service';
import { UISidenav } from 'src/business/shared/ui-sidenavs.enum';
import { UISidenavAction } from '../../../../business/shared/ui-sidenavs.enum';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {

  isCoach$: Observable<boolean>;

  constructor(
    private uiService: UIService,
    private router: Router
  ) { }

  ngOnInit() {
  }

  //TODO: Event source this out
  public close() {
    this.uiService.doSidenavAction(UISidenav.App, UISidenavAction.Close);
  }

  public onRoute(route: string) {
    this.uiService.doSidenavAction(UISidenav.App, UISidenavAction.Toggle);
    setTimeout(_ => this.router.navigate([route]), 400);
  }
}
