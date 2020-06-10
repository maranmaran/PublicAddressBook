using Backend.Domain.Entities.Contacts;
using System;

namespace Backend.Persistence.Seed
{
    public static class ContactsSeeder
    {
        public static Contact[] GetContacts()
        {
            var contacts = new Contact[]
            {
                new Contact
                {
                    Id = Guid.NewGuid(),
                    Name = "Dorise Grosvenor",
                    Address = "73588 Maywood Drive",
                },
                new Contact
                {
                    Id = Guid.NewGuid(),
                    Name = "Esma Jostan",
                    Address = "0102 Dottie Point",
                },
                new Contact
                {
                    Id = Guid.NewGuid(),
                    Name = "Flo Greenwell",
                    Address = "56 Kennedy Trail",
                },
                new Contact
                {
                    Id = Guid.NewGuid(),
                    Name = "Titos Malicki",
                    Address = "42502 Carpenter Hill",
                },
                new Contact
                {
                    Id = Guid.NewGuid(),
                    Name = "Guilbert Scullard",
                    Address = "8046 Menomonie Road",
                },
                new Contact
                {
                    Id = Guid.NewGuid(),
                    Name = "Constantine Skirven",
                    Address = "9 Alpine Point",
                },
                new Contact
                {
                    Id = Guid.NewGuid(),
                    Name = "Teresita Kinnett",
                    Address = "0102 Dottie Point",
                },
                new Contact
                {
                    Id = Guid.NewGuid(),
                    Name = "Carmelita Goodding",
                    Address = "69 Maywood Trail",
                },
                new Contact
                {
                    Id = Guid.NewGuid(),
                    Name = "Cleavland Lifsey",
                    Address = "5607 Ridgeway Road",
                },
                new Contact
                {
                    Id = Guid.NewGuid(),
                    Name = "Obadiah Alloway",
                    Address = "839 Riverside Court",
                },
                new Contact
                {
                    Id = Guid.NewGuid(),
                    Name = "Sarah Gouldstraw",
                    Address = "821 Randy Pass",
                },
                new Contact
                {
                    Id = Guid.NewGuid(),
                    Name = "Anita Toth",
                    Address = "3 7th Hill",
                }, new Contact
                {
                    Id = Guid.NewGuid(),
                    Name = "Roselia Stucksbury",
                    Address = "45089 Russell Pass",
                }, new Contact
                {
                    Id = Guid.NewGuid(),
                    Name = "Clement Walkington",
                    Address = "83 Village Green Junction",
                },

            };

            return contacts;
        }
    }
}