import { HttpErrorResponse } from '@angular/common/http';
import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Update } from '@ngrx/entity';
import { Store } from '@ngrx/store';
import { ContactService } from 'src/business/services/features/contacts.service';
import { CRUD } from 'src/business/shared/crud.enum';
import { CreateContactRequest } from 'src/models/contacts/create-contact.request';
import { Contact } from 'src/models/domain-entities/contact.model';
import { ServerStatusCodes } from 'src/models/error/status-codes/server.codes';
import { contactCreated, contactUpdated } from 'src/ngrx/contacts/contact.actions';
import { AppState } from 'src/ngrx/global-setup.ngrx';
import { disableErrorDialogs } from 'src/ngrx/user-interface/ui.actions';
import { PhoneNumberValidatior } from './../../../../../business/validators/phone-number.validator';
import { UpdateContactRequest } from './../../../../../models/contacts/update-contact.request';
import { ValidationErrors } from './../../../../../models/error/error-details.model';
import { enableErrorDialogs } from './../../../../../ngrx/user-interface/ui.actions';

@Component({
  selector: 'app-contact-create-edit',
  templateUrl: './contact-create-edit.component.html',
  styleUrls: ['./contact-create-edit.component.scss']
})
export class ContactCreateEditComponent implements OnInit, OnDestroy {


  constructor(
    private store: Store<AppState>,
    private contactService: ContactService,
    private dialogRef: MatDialogRef<ContactCreateEditComponent>,
    private phoneValidator: PhoneNumberValidatior,
    @Inject(MAT_DIALOG_DATA) public data: { title: string, action: CRUD, contact: Contact }) { }

  form: FormGroup;
  contact = new Contact();

  ngOnInit() {
    this.store.dispatch(disableErrorDialogs())

    if (this.data.action == CRUD.Update) this.contact = Object.assign({}, this.data.contact);

    this.createForm();
  }

  ngOnDestroy() {
    this.store.dispatch(enableErrorDialogs())
  }

  createForm() {
    this.form = new FormGroup({
      name: new FormControl(this.contact.name, [Validators.required]),
      address: new FormControl(this.contact.address, [Validators.required]),
      phoneNumbers: new FormArray([])
    });

    if (!this.contact.phoneNumbers || this.contact.phoneNumbers.length == 0) {
      this.phoneNumbers.push(new FormControl(''));
    } else {
      this.contact.phoneNumbers.forEach(numberObj => {
        this.phoneNumbers.push(new FormControl(numberObj.number, [this.phoneValidator.isValidPhoneNumber.bind(this.phoneValidator)]))
      });
    }

    this.form.updateValueAndValidity();
  }

  get name(): AbstractControl { return this.form.get('name'); }
  get address(): AbstractControl { return this.form.get('address'); }
  get phoneNumbers(): FormArray { return <FormArray>this.form.get('phoneNumbers'); }

  onSubmit() {
    if (this.data.action == CRUD.Create) {
      this.createContact();
    } else {
      this.updateContact();
    }
  }

  addPhoneControl() {
    this.phoneNumbers.push(new FormControl('', this.phoneValidator.isValidPhoneNumber.bind(this.phoneValidator)));
  }

  removePhoneControl(index: number) {
    this.phoneNumbers.controls.splice(index, 1);
    this.phoneNumbers.updateValueAndValidity();
  }

  onClose(contact?: Contact) {
    this.dialogRef.close(contact);
  }

  globalErrors = []
  handleError(validationErrors: ValidationErrors) {
    if (validationErrors.status && validationErrors.status == ServerStatusCodes.ValidationError) {

      // todo make method to outsource repeating code
      if (validationErrors.errors.unique) {
        let errors = new ValidationErrors();

        validationErrors.errors.unique.forEach(err => {
          errors[err] = true;
        });

        this.globalErrors.push(...Object.keys(errors));
      }

      if (validationErrors.errors.name) {
        let errors = new ValidationErrors();

        validationErrors.errors.name.forEach(err => {
          errors[err] = true;
        });

        this.name.setErrors(errors)
      }

      if (validationErrors.errors.address) {
        let errors = new ValidationErrors();

        validationErrors.errors.address.forEach(err => {
          errors[err] = true;
        });

        this.address.setErrors(errors)
      }

      if (validationErrors.errors.phoneNumbers) {
        let errors = new ValidationErrors();

        validationErrors.errors.phoneNumbers.forEach(err => {
          errors[err] = true;
        });

        this.phoneNumbers.setErrors(errors)
      }

    }
  }

  getErrorMessages(key: string) {

    let group = this.form.get(key) as AbstractControl;

    if (!group || !group.valid || !group.errors)
      return null;

    return Object.keys(group.errors).map(key => "VALIDATION." + (key as string).toUpperCase());
  }

  createContact() {
    if (this.form.valid) {
      var request = new CreateContactRequest();
      request.name = this.name.value;
      request.address = this.address.value;

      let phoneNumbers = this.phoneNumbers.value as string[];
      let phoneNumbersFiltered = phoneNumbers.filter(x => x.trim() != "");

      request.phoneNumbers = phoneNumbersFiltered;

      this.contactService.create(request)
        .subscribe(
          (contact: Contact) => {
            this.store.dispatch(contactCreated({ entity: contact }));
            this.onClose(contact);
          },
          (err: HttpErrorResponse) => this.handleError(err.error)
        );
    }
  }

  updateContact() {
    if (this.form.valid) {
      var request = new UpdateContactRequest();
      request.id = this.contact.id;
      request.name = this.name.value;
      request.address = this.address.value;
      request.phoneNumbers = this.phoneNumbers.value;

      this.contactService.update(request)
        .subscribe(
          (contact: Contact) => {
            let updateStatement: Update<Contact> = {
              id: contact.id,
              changes: contact
            }
            this.store.dispatch(contactUpdated({ entity: updateStatement }));
            this.onClose(contact);
          },
          (err: HttpErrorResponse) => this.handleError(err.error)
        );
    }
  }

  formValid() {
    return this.form.valid && this.phoneNumbers.valid;
  }
}