import { createFeatureSelector, createSelector } from '@ngrx/store';
import * as fromContact from './contact.reducers';
import { ContactState } from './contact.state';


export const selectContactState = createFeatureSelector<ContactState>("contact");

export const contactIds = createSelector(
    selectContactState,
    fromContact.selectIds
);

export const contactEntities = createSelector(
    selectContactState,
    fromContact.selectEntities
);

export const contacts = createSelector(
    selectContactState,
    (state) => ({ entities: state ? fromContact.selectAll(state) : [], totalItems: state?.totalItems, pagingModel: state?.pagingModel }),
    // (state, entities) => ({ entities: entities, totalItems: state.totalItems })
);

export const contactCount = createSelector(
    selectContactState,
    fromContact.selectTotal
);

// ids
export const selectedContactId = createSelector(
    selectContactState,
    fromContact.getSelectedContactId
);

export const selectedContact = createSelector(
    selectContactState,
    (state) => state.entities[state.selectedContactId]
);
