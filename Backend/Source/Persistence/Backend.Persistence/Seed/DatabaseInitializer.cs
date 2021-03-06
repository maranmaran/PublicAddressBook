﻿using Backend.Domain.Entities.Contacts;
using Backend.Domain.Entities.User;
using Backend.Domain.Enum;
using Backend.Domain.Factories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Backend.Persistence.Seed
{
    public static class DatabaseInitializer
    {
        public static void Seed(this ModelBuilder b)
        {
            var users = SeedUsers(b);
            var userSettingIds = SeedUserSettings(b, users.Select(x => x.Id).ToArray(), users.Select(x => x.UserSettingId).ToArray());
            SeedNotificationSettings(b, userSettingIds);
            var contacts = SeedContacts(b);
            SeedPhoneNumbers(b, contacts);
        }

        // USERS
        private static void SeedNotificationSettings(ModelBuilder b, IEnumerable<Guid> userSettingIds)
        {
            // missing last character
            // we'll use index of setting to fill that in
            // Other characters will be removed by counter
            var startGuid = "71691ddc-039f-4606-b614-ff4a19516cd";
            var counter = 0;
            foreach (var userSettingId in userSettingIds)
            {
                if (counter % 10 == 0)
                {
                    startGuid = startGuid.Remove(startGuid.Length - 1);
                }
                var values = EnumFactory.SeedEnum<NotificationType, NotificationSetting>((value, index) => new NotificationSetting()
                {
                    Id = Guid.Parse(startGuid + counter + index),
                    NotificationType = value,
                    UserSettingId = userSettingId
                }).ToList();

                counter++;
                b.Entity<NotificationSetting>().HasData(values);
            }
        }

        private static IEnumerable<Guid> SeedUserSettings(ModelBuilder b, Guid[] userIds, Guid[] settingIds)
        {
            for (int i = 0; i < userIds.Length; i++)
            {
                var userSetting = new UserSetting()
                {
                    Id = settingIds[i],
                    ApplicationUserId = userIds[i],
                };

                b.Entity<UserSetting>().HasData(userSetting);
            }

            return settingIds;
        }

        private static IEnumerable<ApplicationUser> SeedUsers(ModelBuilder b)
        {
            var user = UsersSeeder.GetUsers();

            b.Entity<ApplicationUser>().HasData(user);

            return new List<ApplicationUser>()
            {
                user,
            };
        }

        private static Contact[] SeedContacts(ModelBuilder b)
        {
            var contacts = ContactsSeeder.GetContacts();

            b.Entity<Contact>().HasData(contacts);

            return contacts;

        }

        private static void SeedPhoneNumbers(ModelBuilder b, Contact[] contacts)
        {
            var numbers = new List<PhoneNumber>();
            foreach (var contact in contacts)
            {
                numbers.Add(new PhoneNumber()
                {
                    Id = Guid.NewGuid(),
                    ContactId = contact.Id,
                    Number = "385989077942"
                });
                numbers.Add(new PhoneNumber()
                {
                    Id = Guid.NewGuid(),
                    ContactId = contact.Id,
                    Number = "361-757-3075"
                });
            }

            b.Entity<PhoneNumber>().HasData(numbers);
        }
    }
}