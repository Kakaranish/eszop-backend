﻿// <auto-generated />
using System;
using Identity.API.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Identity.API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Identity.API.Domain.AboutSeller", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("AboutSellers");
                });

            modelBuilder.Entity("Identity.API.Domain.DeliveryAddress", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("City")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ZipCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("DeliveryAddresses");
                });

            modelBuilder.Entity("Identity.API.Domain.ProfileInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("ProfileInfos");
                });

            modelBuilder.Entity("Identity.API.Domain.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("RevokedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Token")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("Identity.API.Domain.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HashedPassword")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastLogin")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LockedUntil")
                        .HasColumnType("datetime2");

                    b.Property<Guid?>("PrimaryDeliveryAddressId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PrimaryDeliveryAddressId")
                        .IsUnique()
                        .HasFilter("[PrimaryDeliveryAddressId] IS NOT NULL");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("14fadc53-fccb-407b-8583-fef444b802ca"),
                            CreatedAt = new DateTime(2021, 1, 8, 19, 42, 20, 463, DateTimeKind.Utc).AddTicks(2162),
                            Email = "admin@mail.com",
                            HashedPassword = "ts3ERCFgDJeTsO9V/Nb1//83THfQBhxYb1YYi6THGLA=|JyCgNG0Qw4dru5Ii5ENRyQ==|10000",
                            Role = "admin",
                            UpdatedAt = new DateTime(2021, 1, 8, 19, 42, 20, 463, DateTimeKind.Utc).AddTicks(2502)
                        });
                });

            modelBuilder.Entity("Identity.API.Domain.AboutSeller", b =>
                {
                    b.HasOne("Identity.API.Domain.User", null)
                        .WithOne("AboutSeller")
                        .HasForeignKey("Identity.API.Domain.AboutSeller", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Identity.API.Domain.DeliveryAddress", b =>
                {
                    b.HasOne("Identity.API.Domain.User", null)
                        .WithMany("DeliveryAddresses")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Identity.API.Domain.ProfileInfo", b =>
                {
                    b.HasOne("Identity.API.Domain.User", "User")
                        .WithOne("ProfileInfo")
                        .HasForeignKey("Identity.API.Domain.ProfileInfo", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Identity.API.Domain.User", b =>
                {
                    b.HasOne("Identity.API.Domain.DeliveryAddress", "PrimaryDeliveryAddress")
                        .WithOne("User")
                        .HasForeignKey("Identity.API.Domain.User", "PrimaryDeliveryAddressId");

                    b.Navigation("PrimaryDeliveryAddress");
                });

            modelBuilder.Entity("Identity.API.Domain.DeliveryAddress", b =>
                {
                    b.Navigation("User");
                });

            modelBuilder.Entity("Identity.API.Domain.User", b =>
                {
                    b.Navigation("AboutSeller");

                    b.Navigation("DeliveryAddresses");

                    b.Navigation("ProfileInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
