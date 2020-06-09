import { createEntityAdapter, EntityState } from '@ngrx/entity';
import { Contact } from 'src/models/domain-entities/contact.model';
import { PagingModel } from './../../app/shared/material-table/table-models/paging.model';

// Exercise property type ENTITY
export interface ContactState extends EntityState<Contact> {
  selectedContactId: string | number;
  totalItems: number;
  pagingModel: PagingModel;
}

// ADAPTERS
export const adapterContact = createEntityAdapter<Contact>();

// INITIAL STATES
export const contactInitialState = adapterContact.getInitialState({ selectedContactId: undefined, totalItems: undefined, pagingModel: undefined });
