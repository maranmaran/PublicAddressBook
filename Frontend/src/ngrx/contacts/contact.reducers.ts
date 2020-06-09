import { Update } from '@ngrx/entity';
import { Action, ActionReducer, createReducer, on } from '@ngrx/store';
import { PagingModel } from 'src/app/shared/material-table/table-models/paging.model';
import { Contact } from 'src/models/domain-entities/contact.model';
import * as ContactActions from './contact.actions';
import { adapterContact, contactInitialState, ContactState } from './contact.state';

export const contactReducer: ActionReducer<ContactState, Action> = createReducer(
  contactInitialState,

  // CREATE
  on(ContactActions.contactCreated, (state: ContactState, payload: { entity: Contact }) => {

    return adapterContact.addOne(payload.entity, {
      ...state,
      totalItems: state.totalItems + 1
    });
  }),

  // UPDATE
  on(ContactActions.contactUpdated, (state: ContactState, payload: { entity: Update<Contact> }) => {
    return adapterContact.updateOne(payload.entity, state);
  }),

  // DELETE
  on(ContactActions.contactDeleted, (state: ContactState, payload: { id: string }) => {
    return adapterContact.removeOne(payload.id, {
      ...state,
      totalItems: state.totalItems - 1
    });
  }),

  // GET ALL PAGED
  on(ContactActions.contactsFetched, (state: ContactState, payload: { entities: Contact[], totalItems: number, pagingModel: PagingModel }) => {
    return {
      ...adapterContact.addAll(payload.entities, state),
      totalItems: payload.totalItems,
      pagingModel: payload.pagingModel
    };
  }),

  // SET SELECTED
  on(ContactActions.setSelectedContact, (state: ContactState, payload: { entity: Contact }) => {
    return {
      ...state,
      selectedContactId: payload.entity ? payload.entity.id : null,
    };
  }),

  on(ContactActions.clearContactState, (state: ContactState) => {
    return undefined;
  }),
);

export const getSelectedContactId = (state: ContactState) => state.selectedContactId;

// get the selectors
export const {
  selectIds,
  selectEntities,
  selectAll,
  selectTotal,
} = adapterContact.getSelectors();
