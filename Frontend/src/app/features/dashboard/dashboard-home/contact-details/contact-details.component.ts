import { Component, OnInit } from '@angular/core';
import { Store } from '@ngrx/store';
import { Observable } from 'rxjs';
import { Contact } from 'src/models/domain-entities/contact.model';
import { selectedContact } from 'src/ngrx/contacts/contact.selectors';
import { AppState } from 'src/ngrx/global-setup.ngrx';

@Component({
  selector: 'app-contact-details',
  templateUrl: './contact-details.component.html',
  styleUrls: ['./contact-details.component.scss']
})
export class ContactDetailsComponent implements OnInit {

  contact$: Observable<Contact>

  constructor(
    private store: Store<AppState>,
  ) { }

  ngOnInit() {
    this.contact$ = this.store.select(selectedContact);
  }

}
