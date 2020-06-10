using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Backend.Persistence.Migrations
{
    public partial class Seed2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "Id", "Address", "Name" },
                values: new object[,]
                {
                    { new Guid("58bc9dc1-d40e-4ab1-84c8-c347cc2fb636"), "73588 Maywood Drive", "Dorise Grosvenor" },
                    { new Guid("3abf86c3-9250-450f-9a32-7a07eaa39c17"), "0102 Dottie Point", "Esma Jostan" },
                    { new Guid("b60c07ea-d485-4e43-ab13-75b4145d4455"), "56 Kennedy Trail", "Flo Greenwell" },
                    { new Guid("db43e9a1-280d-4e7c-8119-09ec3db13c09"), "42502 Carpenter Hill", "Titos Malicki" },
                    { new Guid("99a03a3e-633a-4402-a1ee-42ad7161e732"), "8046 Menomonie Road", "Guilbert Scullard" },
                    { new Guid("08611593-b061-48e0-9762-37badbcfffa3"), "9 Alpine Point", "Constantine Skirven" },
                    { new Guid("378a6f9f-5450-444b-a327-d8c29db3e5c8"), "0102 Dottie Point", "Teresita Kinnett" },
                    { new Guid("97773fb6-e607-4aa6-8f1c-7256546dc01b"), "69 Maywood Trail", "Carmelita Goodding" },
                    { new Guid("be7338ed-0409-4994-a747-beedb3ed4959"), "5607 Ridgeway Road", "Cleavland Lifsey" },
                    { new Guid("d898eaa1-33f7-4099-8a28-14dd72131c6a"), "839 Riverside Court", "Obadiah Alloway" },
                    { new Guid("4bc36c4c-3966-4d26-b729-f9aa5cd41b68"), "821 Randy Pass", "Sarah Gouldstraw" },
                    { new Guid("cb16de07-fbc3-4ab5-9dd5-c20c1eb174f0"), "3 7th Hill", "Anita Toth" },
                    { new Guid("708853fb-0558-450c-becf-37603871043a"), "45089 Russell Pass", "Roselia Stucksbury" },
                    { new Guid("7cc54118-a5c1-4d57-bb5f-8c4e496d68b1"), "83 Village Green Junction", "Clement Walkington" }
                });


            migrationBuilder.InsertData(
                table: "PhoneNumbers",
                columns: new[] { "Id", "ContactId", "Number" },
                values: new object[,]
                {
                    { new Guid("9bef1f04-69d9-407b-b997-d0c6b3eefff2"), new Guid("7cc54118-a5c1-4d57-bb5f-8c4e496d68b1"), "361-757-3075" },
                    { new Guid("cffd1ddb-7f6f-40c9-a05e-0114821d1329"), new Guid("7cc54118-a5c1-4d57-bb5f-8c4e496d68b1"), "385989077942" },
                    { new Guid("2e8f2c81-8333-4c75-a6b2-dfdb61638a2c"), new Guid("708853fb-0558-450c-becf-37603871043a"), "361-757-3075" },
                    { new Guid("061f2ea5-1d42-43c1-9816-3a718c54c1de"), new Guid("708853fb-0558-450c-becf-37603871043a"), "385989077942" },
                    { new Guid("51009bcc-c0d2-4e0e-8289-6c0f6df2a435"), new Guid("cb16de07-fbc3-4ab5-9dd5-c20c1eb174f0"), "361-757-3075" },
                    { new Guid("c8861842-9148-44e0-a609-8e99dfd1e6e6"), new Guid("cb16de07-fbc3-4ab5-9dd5-c20c1eb174f0"), "385989077942" },
                    { new Guid("4dc1d6d2-7160-4280-8224-0a6331c72a52"), new Guid("4bc36c4c-3966-4d26-b729-f9aa5cd41b68"), "361-757-3075" },
                    { new Guid("64c4413a-34a3-4ae1-a5a1-6980966cc566"), new Guid("4bc36c4c-3966-4d26-b729-f9aa5cd41b68"), "385989077942" },
                    { new Guid("6428920c-f013-476c-baf6-2f1410def6ee"), new Guid("d898eaa1-33f7-4099-8a28-14dd72131c6a"), "361-757-3075" },
                    { new Guid("8a9ac32e-eba6-4b20-ba94-cc80bc8c6742"), new Guid("d898eaa1-33f7-4099-8a28-14dd72131c6a"), "385989077942" },
                    { new Guid("a6f91d5e-636b-4cfb-853a-31ccdca7cbbc"), new Guid("be7338ed-0409-4994-a747-beedb3ed4959"), "361-757-3075" },
                    { new Guid("1b3100e0-f327-4541-9c0e-93c0395c44ed"), new Guid("be7338ed-0409-4994-a747-beedb3ed4959"), "385989077942" },
                    { new Guid("256287ce-24de-4f3c-8c74-5b10e09508b1"), new Guid("97773fb6-e607-4aa6-8f1c-7256546dc01b"), "361-757-3075" },
                    { new Guid("cddf7128-a834-4fe9-abbb-cddbcceb72ba"), new Guid("97773fb6-e607-4aa6-8f1c-7256546dc01b"), "385989077942" },
                    { new Guid("14e859b5-c9ef-44c0-8261-043bdf125966"), new Guid("378a6f9f-5450-444b-a327-d8c29db3e5c8"), "361-757-3075" },
                    { new Guid("01cdac0c-f481-476e-854c-0f9096352e1b"), new Guid("378a6f9f-5450-444b-a327-d8c29db3e5c8"), "385989077942" },
                    { new Guid("669d3991-3add-4251-9316-15d9feb29b51"), new Guid("08611593-b061-48e0-9762-37badbcfffa3"), "361-757-3075" },
                    { new Guid("7c3a6293-aa15-4c9b-9762-bc285c5d2527"), new Guid("08611593-b061-48e0-9762-37badbcfffa3"), "385989077942" },
                    { new Guid("19f47ebe-01b4-4cd1-8f4a-6ecc7ec26691"), new Guid("99a03a3e-633a-4402-a1ee-42ad7161e732"), "361-757-3075" },
                    { new Guid("c4318deb-9668-4317-a20e-a20cb5b3eb37"), new Guid("99a03a3e-633a-4402-a1ee-42ad7161e732"), "385989077942" },
                    { new Guid("73be7142-c829-4329-9e43-986a0d2d9af2"), new Guid("db43e9a1-280d-4e7c-8119-09ec3db13c09"), "361-757-3075" },
                    { new Guid("b13008ee-165a-4fae-8ab9-90112b6ad4b5"), new Guid("db43e9a1-280d-4e7c-8119-09ec3db13c09"), "385989077942" },
                    { new Guid("cf2fe539-3992-4abe-bae6-73edc1162ef5"), new Guid("b60c07ea-d485-4e43-ab13-75b4145d4455"), "361-757-3075" },
                    { new Guid("645e1602-0be4-4e89-bcfc-acc03b9c012f"), new Guid("b60c07ea-d485-4e43-ab13-75b4145d4455"), "385989077942" },
                    { new Guid("9ee8254c-208e-4088-89f4-e4e6c027a5ec"), new Guid("3abf86c3-9250-450f-9a32-7a07eaa39c17"), "361-757-3075" },
                    { new Guid("3b94c6ab-d3ff-44b2-a337-c579987fa4ff"), new Guid("3abf86c3-9250-450f-9a32-7a07eaa39c17"), "385989077942" },
                    { new Guid("ee952eaf-fccc-4780-bd2d-9096a6b5baf7"), new Guid("58bc9dc1-d40e-4ab1-84c8-c347cc2fb636"), "361-757-3075" },
                    { new Guid("c4cefb71-8e02-4e69-9b11-67a3f0ce8c41"), new Guid("58bc9dc1-d40e-4ab1-84c8-c347cc2fb636"), "385989077942" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "Id",
                keyValue: new Guid("01cdac0c-f481-476e-854c-0f9096352e1b"));

            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "Id",
                keyValue: new Guid("061f2ea5-1d42-43c1-9816-3a718c54c1de"));

            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "Id",
                keyValue: new Guid("14e859b5-c9ef-44c0-8261-043bdf125966"));

            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "Id",
                keyValue: new Guid("19f47ebe-01b4-4cd1-8f4a-6ecc7ec26691"));

            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "Id",
                keyValue: new Guid("1b3100e0-f327-4541-9c0e-93c0395c44ed"));

            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "Id",
                keyValue: new Guid("256287ce-24de-4f3c-8c74-5b10e09508b1"));

            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "Id",
                keyValue: new Guid("2e8f2c81-8333-4c75-a6b2-dfdb61638a2c"));

            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "Id",
                keyValue: new Guid("3b94c6ab-d3ff-44b2-a337-c579987fa4ff"));

            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "Id",
                keyValue: new Guid("4dc1d6d2-7160-4280-8224-0a6331c72a52"));

            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "Id",
                keyValue: new Guid("51009bcc-c0d2-4e0e-8289-6c0f6df2a435"));

            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "Id",
                keyValue: new Guid("6428920c-f013-476c-baf6-2f1410def6ee"));

            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "Id",
                keyValue: new Guid("645e1602-0be4-4e89-bcfc-acc03b9c012f"));

            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "Id",
                keyValue: new Guid("64c4413a-34a3-4ae1-a5a1-6980966cc566"));

            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "Id",
                keyValue: new Guid("669d3991-3add-4251-9316-15d9feb29b51"));

            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "Id",
                keyValue: new Guid("73be7142-c829-4329-9e43-986a0d2d9af2"));

            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "Id",
                keyValue: new Guid("7c3a6293-aa15-4c9b-9762-bc285c5d2527"));

            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "Id",
                keyValue: new Guid("8a9ac32e-eba6-4b20-ba94-cc80bc8c6742"));

            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "Id",
                keyValue: new Guid("9bef1f04-69d9-407b-b997-d0c6b3eefff2"));

            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "Id",
                keyValue: new Guid("9ee8254c-208e-4088-89f4-e4e6c027a5ec"));

            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "Id",
                keyValue: new Guid("a6f91d5e-636b-4cfb-853a-31ccdca7cbbc"));

            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "Id",
                keyValue: new Guid("b13008ee-165a-4fae-8ab9-90112b6ad4b5"));

            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "Id",
                keyValue: new Guid("c4318deb-9668-4317-a20e-a20cb5b3eb37"));

            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "Id",
                keyValue: new Guid("c4cefb71-8e02-4e69-9b11-67a3f0ce8c41"));

            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "Id",
                keyValue: new Guid("c8861842-9148-44e0-a609-8e99dfd1e6e6"));

            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "Id",
                keyValue: new Guid("cddf7128-a834-4fe9-abbb-cddbcceb72ba"));

            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "Id",
                keyValue: new Guid("cf2fe539-3992-4abe-bae6-73edc1162ef5"));

            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "Id",
                keyValue: new Guid("cffd1ddb-7f6f-40c9-a05e-0114821d1329"));

            migrationBuilder.DeleteData(
                table: "PhoneNumbers",
                keyColumn: "Id",
                keyValue: new Guid("ee952eaf-fccc-4780-bd2d-9096a6b5baf7"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("8d399c00-5654-4a54-9c2c-b44a485c3583"));

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: new Guid("08611593-b061-48e0-9762-37badbcfffa3"));

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: new Guid("378a6f9f-5450-444b-a327-d8c29db3e5c8"));

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: new Guid("3abf86c3-9250-450f-9a32-7a07eaa39c17"));

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: new Guid("4bc36c4c-3966-4d26-b729-f9aa5cd41b68"));

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: new Guid("58bc9dc1-d40e-4ab1-84c8-c347cc2fb636"));

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: new Guid("708853fb-0558-450c-becf-37603871043a"));

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: new Guid("7cc54118-a5c1-4d57-bb5f-8c4e496d68b1"));

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: new Guid("97773fb6-e607-4aa6-8f1c-7256546dc01b"));

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: new Guid("99a03a3e-633a-4402-a1ee-42ad7161e732"));

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: new Guid("b60c07ea-d485-4e43-ab13-75b4145d4455"));

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: new Guid("be7338ed-0409-4994-a747-beedb3ed4959"));

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: new Guid("cb16de07-fbc3-4ab5-9dd5-c20c1eb174f0"));

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: new Guid("d898eaa1-33f7-4099-8a28-14dd72131c6a"));

            migrationBuilder.DeleteData(
                table: "Contacts",
                keyColumn: "Id",
                keyValue: new Guid("db43e9a1-280d-4e7c-8119-09ec3db13c09"));
        }
    }
}
