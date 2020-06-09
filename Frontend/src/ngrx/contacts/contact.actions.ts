import { Update } from '@ngrx/entity';
import { createAction, props } from "@ngrx/store";
import { PagingModel } from '../../app/shared/material-table/table-models/paging.model';
import { Contact } from '../../models/domain-entities/contact.model';


export const contactCreated = createAction(
  '[Contact] Created',
  props<{ entity: Contact }>()
);

export const contactsFetched = createAction(
  '[Contact] Fetched',
  props<{ entities: Contact[], totalItems: number, pagingModel: PagingModel }>()
);

export const contactUpdated = createAction(
  '[Contact] Updated',
  props<{ entity: Update<Contact> }>()
);

export const contactDeleted = createAction(
  '[Contact] Deleted',
  props<{ id: string }>()
);

export const setSelectedContact = createAction(
  '[Contact] Set selected contact',
  props<{ entity: Contact }>()
);

export const clearContactState = createAction(
  '[Contact] Clear',
);
