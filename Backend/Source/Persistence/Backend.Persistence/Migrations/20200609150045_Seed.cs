using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Persistence.Migrations
{
    public partial class Seed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "UserSettings",
                columns: new[] { "Id", "ApplicationUserId" },
                values: new object[,]
                {
                    { new Guid("0d528a91-fbbe-4a02-924a-792344bbbd65"), new Guid("0faee6ac-1772-4bbe-9990-a7d9a22dd529") },
                    { new Guid("8d399c00-5684-4a54-9c2c-b44a485c3583"), new Guid("8d399c00-5654-4a54-9c2c-b44a485c3583") }
                });

            migrationBuilder.InsertData(
                table: "NotificationSetting",
                columns: new[] { "Id", "NotificationType", "UserSettingId" },
                values: new object[,]
                {
                    { new Guid("71691ddc-039f-4606-b614-ff4a19516c00"), "MediaAdded", new Guid("0d528a91-fbbe-4a02-924a-792344bbbd65") },
                    { new Guid("71691ddc-039f-4606-b614-ff4a19516c01"), "ContactChanged", new Guid("0d528a91-fbbe-4a02-924a-792344bbbd65") },
                    { new Guid("71691ddc-039f-4606-b614-ff4a19516c10"), "MediaAdded", new Guid("8d399c00-5684-4a54-9c2c-b44a485c3583") },
                    { new Guid("71691ddc-039f-4606-b614-ff4a19516c11"), "ContactChanged", new Guid("8d399c00-5684-4a54-9c2c-b44a485c3583") }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AccountType", "Avatar", "CreatedOn", "CustomerId", "Email", "FirstName", "LastName", "PasswordHash", "UserSettingId", "Username" },
                values: new object[,]
                {
                    { new Guid("0faee6ac-1772-4bbe-9990-a7d9a22dd529"), "Admin", "https://ui-avatars.com/api/?name=Admin+&rounded=True&background=06BAE6&color=ffffff&", new DateTime(2020, 6, 9, 15, 0, 44, 442, DateTimeKind.Utc).AddTicks(3848), "cus_FLi7gZv8w0j0GB", "admin@trainingcompanion.com", "Admin", "", "8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918", new Guid("0d528a91-fbbe-4a02-924a-792344bbbd65"), "admin" },
                    { new Guid("8d399c00-5654-4a54-9c2c-b44a485c3583"), "User", "https://ui-avatars.com/api/?name=Firstname+Lastname&rounded=True&background=E7FCA4&color=ffffff&", new DateTime(2020, 6, 9, 15, 0, 44, 442, DateTimeKind.Utc).AddTicks(5665), "cus_FHk5RepADdfm5H", "user@application.com", "Firstname", "Lastname", "04f8996da763b7a969b1028ee3007569eaf3a635486ddab211d512c85b9df8fb", new Guid("8d399c00-5684-4a54-9c2c-b44a485c3583"), "user" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "NotificationSetting",
                keyColumn: "Id",
                keyValue: new Guid("71691ddc-039f-4606-b614-ff4a19516c00"));

            migrationBuilder.DeleteData(
                table: "NotificationSetting",
                keyColumn: "Id",
                keyValue: new Guid("71691ddc-039f-4606-b614-ff4a19516c01"));

            migrationBuilder.DeleteData(
                table: "NotificationSetting",
                keyColumn: "Id",
                keyValue: new Guid("71691ddc-039f-4606-b614-ff4a19516c10"));

            migrationBuilder.DeleteData(
                table: "NotificationSetting",
                keyColumn: "Id",
                keyValue: new Guid("71691ddc-039f-4606-b614-ff4a19516c11"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("0faee6ac-1772-4bbe-9990-a7d9a22dd529"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8d399c00-5654-4a54-9c2c-b44a485c3583"));

            migrationBuilder.DeleteData(
                table: "UserSettings",
                keyColumn: "Id",
                keyValue: new Guid("0d528a91-fbbe-4a02-924a-792344bbbd65"));

            migrationBuilder.DeleteData(
                table: "UserSettings",
                keyColumn: "Id",
                keyValue: new Guid("8d399c00-5684-4a54-9c2c-b44a485c3583"));
        }
    }
}
