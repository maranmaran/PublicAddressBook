﻿// <auto-generated />
using System;
using Backend.Domain.Enum;
using Backend.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Backend.Persistence.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Backend.Domain.Entities.Auditing.AuditRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Action")
                        .HasColumnType("text");

                    b.Property<string>("Data")
                        .HasColumnType("text");

                    b.Property<DateTime>("Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("EntityType")
                        .HasColumnType("text");

                    b.Property<string>("PrimaryKey")
                        .HasColumnType("text");

                    b.Property<bool>("Seen")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<string>("Table")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PrimaryKey");

                    b.HasIndex("UserId");

                    b.ToTable("Audits");
                });

            modelBuilder.Entity("Backend.Domain.Entities.Contacts.Contact", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .HasColumnType("character varying(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Name")
                        .HasColumnType("character varying(200)")
                        .HasMaxLength(200);

                    b.HasKey("Id");

                    b.HasIndex("Name", "Address")
                        .IsUnique();

                    b.ToTable("Contacts");

                    b.HasData(
                        new
                        {
                            Id = new Guid("58bc9dc1-d40e-4ab1-84c8-c347cc2fb636"),
                            Address = "73588 Maywood Drive",
                            Name = "Dorise Grosvenor"
                        },
                        new
                        {
                            Id = new Guid("3abf86c3-9250-450f-9a32-7a07eaa39c17"),
                            Address = "0102 Dottie Point",
                            Name = "Esma Jostan"
                        },
                        new
                        {
                            Id = new Guid("b60c07ea-d485-4e43-ab13-75b4145d4455"),
                            Address = "56 Kennedy Trail",
                            Name = "Flo Greenwell"
                        },
                        new
                        {
                            Id = new Guid("db43e9a1-280d-4e7c-8119-09ec3db13c09"),
                            Address = "42502 Carpenter Hill",
                            Name = "Titos Malicki"
                        },
                        new
                        {
                            Id = new Guid("99a03a3e-633a-4402-a1ee-42ad7161e732"),
                            Address = "8046 Menomonie Road",
                            Name = "Guilbert Scullard"
                        },
                        new
                        {
                            Id = new Guid("08611593-b061-48e0-9762-37badbcfffa3"),
                            Address = "9 Alpine Point",
                            Name = "Constantine Skirven"
                        },
                        new
                        {
                            Id = new Guid("378a6f9f-5450-444b-a327-d8c29db3e5c8"),
                            Address = "0102 Dottie Point",
                            Name = "Teresita Kinnett"
                        },
                        new
                        {
                            Id = new Guid("97773fb6-e607-4aa6-8f1c-7256546dc01b"),
                            Address = "69 Maywood Trail",
                            Name = "Carmelita Goodding"
                        },
                        new
                        {
                            Id = new Guid("be7338ed-0409-4994-a747-beedb3ed4959"),
                            Address = "5607 Ridgeway Road",
                            Name = "Cleavland Lifsey"
                        },
                        new
                        {
                            Id = new Guid("d898eaa1-33f7-4099-8a28-14dd72131c6a"),
                            Address = "839 Riverside Court",
                            Name = "Obadiah Alloway"
                        },
                        new
                        {
                            Id = new Guid("4bc36c4c-3966-4d26-b729-f9aa5cd41b68"),
                            Address = "821 Randy Pass",
                            Name = "Sarah Gouldstraw"
                        },
                        new
                        {
                            Id = new Guid("cb16de07-fbc3-4ab5-9dd5-c20c1eb174f0"),
                            Address = "3 7th Hill",
                            Name = "Anita Toth"
                        },
                        new
                        {
                            Id = new Guid("708853fb-0558-450c-becf-37603871043a"),
                            Address = "45089 Russell Pass",
                            Name = "Roselia Stucksbury"
                        },
                        new
                        {
                            Id = new Guid("7cc54118-a5c1-4d57-bb5f-8c4e496d68b1"),
                            Address = "83 Village Green Junction",
                            Name = "Clement Walkington"
                        });
                });

            modelBuilder.Entity("Backend.Domain.Entities.Contacts.PhoneNumber", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ContactId")
                        .HasColumnType("uuid");

                    b.Property<string>("Number")
                        .HasColumnType("character varying(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("ContactId");

                    b.ToTable("PhoneNumbers");

                    b.HasData(
                        new
                        {
                            Id = new Guid("c4cefb71-8e02-4e69-9b11-67a3f0ce8c41"),
                            ContactId = new Guid("58bc9dc1-d40e-4ab1-84c8-c347cc2fb636"),
                            Number = "385989077942"
                        },
                        new
                        {
                            Id = new Guid("ee952eaf-fccc-4780-bd2d-9096a6b5baf7"),
                            ContactId = new Guid("58bc9dc1-d40e-4ab1-84c8-c347cc2fb636"),
                            Number = "361-757-3075"
                        },
                        new
                        {
                            Id = new Guid("3b94c6ab-d3ff-44b2-a337-c579987fa4ff"),
                            ContactId = new Guid("3abf86c3-9250-450f-9a32-7a07eaa39c17"),
                            Number = "385989077942"
                        },
                        new
                        {
                            Id = new Guid("9ee8254c-208e-4088-89f4-e4e6c027a5ec"),
                            ContactId = new Guid("3abf86c3-9250-450f-9a32-7a07eaa39c17"),
                            Number = "361-757-3075"
                        },
                        new
                        {
                            Id = new Guid("645e1602-0be4-4e89-bcfc-acc03b9c012f"),
                            ContactId = new Guid("b60c07ea-d485-4e43-ab13-75b4145d4455"),
                            Number = "385989077942"
                        },
                        new
                        {
                            Id = new Guid("cf2fe539-3992-4abe-bae6-73edc1162ef5"),
                            ContactId = new Guid("b60c07ea-d485-4e43-ab13-75b4145d4455"),
                            Number = "361-757-3075"
                        },
                        new
                        {
                            Id = new Guid("b13008ee-165a-4fae-8ab9-90112b6ad4b5"),
                            ContactId = new Guid("db43e9a1-280d-4e7c-8119-09ec3db13c09"),
                            Number = "385989077942"
                        },
                        new
                        {
                            Id = new Guid("73be7142-c829-4329-9e43-986a0d2d9af2"),
                            ContactId = new Guid("db43e9a1-280d-4e7c-8119-09ec3db13c09"),
                            Number = "361-757-3075"
                        },
                        new
                        {
                            Id = new Guid("c4318deb-9668-4317-a20e-a20cb5b3eb37"),
                            ContactId = new Guid("99a03a3e-633a-4402-a1ee-42ad7161e732"),
                            Number = "385989077942"
                        },
                        new
                        {
                            Id = new Guid("19f47ebe-01b4-4cd1-8f4a-6ecc7ec26691"),
                            ContactId = new Guid("99a03a3e-633a-4402-a1ee-42ad7161e732"),
                            Number = "361-757-3075"
                        },
                        new
                        {
                            Id = new Guid("7c3a6293-aa15-4c9b-9762-bc285c5d2527"),
                            ContactId = new Guid("08611593-b061-48e0-9762-37badbcfffa3"),
                            Number = "385989077942"
                        },
                        new
                        {
                            Id = new Guid("669d3991-3add-4251-9316-15d9feb29b51"),
                            ContactId = new Guid("08611593-b061-48e0-9762-37badbcfffa3"),
                            Number = "361-757-3075"
                        },
                        new
                        {
                            Id = new Guid("01cdac0c-f481-476e-854c-0f9096352e1b"),
                            ContactId = new Guid("378a6f9f-5450-444b-a327-d8c29db3e5c8"),
                            Number = "385989077942"
                        },
                        new
                        {
                            Id = new Guid("14e859b5-c9ef-44c0-8261-043bdf125966"),
                            ContactId = new Guid("378a6f9f-5450-444b-a327-d8c29db3e5c8"),
                            Number = "361-757-3075"
                        },
                        new
                        {
                            Id = new Guid("cddf7128-a834-4fe9-abbb-cddbcceb72ba"),
                            ContactId = new Guid("97773fb6-e607-4aa6-8f1c-7256546dc01b"),
                            Number = "385989077942"
                        },
                        new
                        {
                            Id = new Guid("256287ce-24de-4f3c-8c74-5b10e09508b1"),
                            ContactId = new Guid("97773fb6-e607-4aa6-8f1c-7256546dc01b"),
                            Number = "361-757-3075"
                        },
                        new
                        {
                            Id = new Guid("1b3100e0-f327-4541-9c0e-93c0395c44ed"),
                            ContactId = new Guid("be7338ed-0409-4994-a747-beedb3ed4959"),
                            Number = "385989077942"
                        },
                        new
                        {
                            Id = new Guid("a6f91d5e-636b-4cfb-853a-31ccdca7cbbc"),
                            ContactId = new Guid("be7338ed-0409-4994-a747-beedb3ed4959"),
                            Number = "361-757-3075"
                        },
                        new
                        {
                            Id = new Guid("8a9ac32e-eba6-4b20-ba94-cc80bc8c6742"),
                            ContactId = new Guid("d898eaa1-33f7-4099-8a28-14dd72131c6a"),
                            Number = "385989077942"
                        },
                        new
                        {
                            Id = new Guid("6428920c-f013-476c-baf6-2f1410def6ee"),
                            ContactId = new Guid("d898eaa1-33f7-4099-8a28-14dd72131c6a"),
                            Number = "361-757-3075"
                        },
                        new
                        {
                            Id = new Guid("64c4413a-34a3-4ae1-a5a1-6980966cc566"),
                            ContactId = new Guid("4bc36c4c-3966-4d26-b729-f9aa5cd41b68"),
                            Number = "385989077942"
                        },
                        new
                        {
                            Id = new Guid("4dc1d6d2-7160-4280-8224-0a6331c72a52"),
                            ContactId = new Guid("4bc36c4c-3966-4d26-b729-f9aa5cd41b68"),
                            Number = "361-757-3075"
                        },
                        new
                        {
                            Id = new Guid("c8861842-9148-44e0-a609-8e99dfd1e6e6"),
                            ContactId = new Guid("cb16de07-fbc3-4ab5-9dd5-c20c1eb174f0"),
                            Number = "385989077942"
                        },
                        new
                        {
                            Id = new Guid("51009bcc-c0d2-4e0e-8289-6c0f6df2a435"),
                            ContactId = new Guid("cb16de07-fbc3-4ab5-9dd5-c20c1eb174f0"),
                            Number = "361-757-3075"
                        },
                        new
                        {
                            Id = new Guid("061f2ea5-1d42-43c1-9816-3a718c54c1de"),
                            ContactId = new Guid("708853fb-0558-450c-becf-37603871043a"),
                            Number = "385989077942"
                        },
                        new
                        {
                            Id = new Guid("2e8f2c81-8333-4c75-a6b2-dfdb61638a2c"),
                            ContactId = new Guid("708853fb-0558-450c-becf-37603871043a"),
                            Number = "361-757-3075"
                        },
                        new
                        {
                            Id = new Guid("cffd1ddb-7f6f-40c9-a05e-0114821d1329"),
                            ContactId = new Guid("7cc54118-a5c1-4d57-bb5f-8c4e496d68b1"),
                            Number = "385989077942"
                        },
                        new
                        {
                            Id = new Guid("9bef1f04-69d9-407b-b997-d0c6b3eefff2"),
                            ContactId = new Guid("7cc54118-a5c1-4d57-bb5f-8c4e496d68b1"),
                            Number = "361-757-3075"
                        });
                });

            modelBuilder.Entity("Backend.Domain.Entities.Media.MediaFile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ApplicationUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime>("DateUploaded")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DownloadUrl")
                        .HasColumnType("text");

                    b.Property<string>("Filename")
                        .HasColumnType("text");

                    b.Property<string>("S3FilePath")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("MediaFiles");
                });

            modelBuilder.Entity("Backend.Domain.Entities.Notification.Notification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("JsonEntity")
                        .HasColumnType("text");

                    b.Property<string>("Payload")
                        .HasColumnType("text");

                    b.Property<bool>("Read")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<Guid?>("ReceiverId")
                        .HasColumnType("uuid");

                    b.Property<string>("RedirectUrl")
                        .HasColumnType("text");

                    b.Property<string>("SenderAvatar")
                        .HasColumnType("text");

                    b.Property<Guid?>("SenderId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("SentAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("Subtype")
                        .HasColumnType("text");

                    b.Property<bool>("SystemNotification")
                        .HasColumnType("boolean");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("SenderId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("Backend.Domain.Entities.System.SystemLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Date")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("InnerException")
                        .HasColumnType("text");

                    b.Property<string>("LogLevel")
                        .HasColumnType("text");

                    b.Property<string>("Message")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("SystemLog");
                });

            modelBuilder.Entity("Backend.Domain.Entities.User.ApplicationUser", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AccountType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("Active")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<string>("Avatar")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("Male");

                    b.Property<DateTime>("LastModified")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("NOW()");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<Guid>("UserSettingId")
                        .HasColumnType("uuid");

                    b.Property<string>("Username")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserSettingId")
                        .IsUnique();

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("AccountType").HasValue("User");

                    b.HasData(
                        new
                        {
                            Id = new Guid("8d399c00-5654-4a54-9c2c-b44a485c3583"),
                            AccountType = "Admin",
                            Active = false,
                            CreatedOn = new DateTime(2020, 6, 10, 1, 30, 8, 23, DateTimeKind.Utc).AddTicks(2741),
                            Email = "user@application.com",
                            FirstName = "Firstname",
                            Gender = "Male",
                            LastModified = new DateTime(2020, 6, 10, 1, 30, 8, 23, DateTimeKind.Utc).AddTicks(3091),
                            LastName = "Lastname",
                            PasswordHash = "04f8996da763b7a969b1028ee3007569eaf3a635486ddab211d512c85b9df8fb",
                            UserSettingId = new Guid("8d399c00-5684-4a54-9c2c-b44a485c3583"),
                            Username = "user"
                        });
                });

            modelBuilder.Entity("Backend.Domain.Entities.User.NotificationSetting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("NotificationType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("ReceiveNotification")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true);

                    b.Property<Guid>("UserSettingId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserSettingId");

                    b.ToTable("NotificationSetting");

                    b.HasData(
                        new
                        {
                            Id = new Guid("71691ddc-039f-4606-b614-ff4a19516c00"),
                            NotificationType = "ContactChanged",
                            ReceiveNotification = false,
                            UserSettingId = new Guid("8d399c00-5684-4a54-9c2c-b44a485c3583")
                        });
                });

            modelBuilder.Entity("Backend.Domain.Entities.User.UserSetting", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ApplicationUserId")
                        .HasColumnType("uuid");

                    b.Property<string>("Language")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("character varying(2)")
                        .HasMaxLength(2)
                        .HasDefaultValue("en");

                    b.Property<string>("Theme")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("Light");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.ToTable("UserSettings");

                    b.HasData(
                        new
                        {
                            Id = new Guid("8d399c00-5684-4a54-9c2c-b44a485c3583"),
                            ApplicationUserId = new Guid("8d399c00-5654-4a54-9c2c-b44a485c3583"),
                            Theme = "Light"
                        });
                });

            modelBuilder.Entity("Backend.Domain.Entities.Contacts.PhoneNumber", b =>
                {
                    b.HasOne("Backend.Domain.Entities.Contacts.Contact", "Contact")
                        .WithMany("PhoneNumbers")
                        .HasForeignKey("ContactId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Backend.Domain.Entities.Media.MediaFile", b =>
                {
                    b.HasOne("Backend.Domain.Entities.User.ApplicationUser", "ApplicationUser")
                        .WithMany("MediaFiles")
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Backend.Domain.Entities.Notification.Notification", b =>
                {
                    b.HasOne("Backend.Domain.Entities.User.ApplicationUser", "Receiver")
                        .WithMany("ReceivedNotifications")
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Backend.Domain.Entities.User.ApplicationUser", "Sender")
                        .WithMany("SentNotifications")
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Backend.Domain.Entities.User.ApplicationUser", b =>
                {
                    b.HasOne("Backend.Domain.Entities.User.UserSetting", "UserSetting")
                        .WithOne("ApplicationUser")
                        .HasForeignKey("Backend.Domain.Entities.User.ApplicationUser", "UserSettingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Backend.Domain.Entities.User.NotificationSetting", b =>
                {
                    b.HasOne("Backend.Domain.Entities.User.UserSetting", "UserSetting")
                        .WithMany("NotificationSettings")
                        .HasForeignKey("UserSettingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
