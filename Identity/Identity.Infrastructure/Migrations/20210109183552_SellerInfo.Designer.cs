﻿// <auto-generated />
using System;
using Identity.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Identity.API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20210109183552_SellerInfo")]
    partial class SellerInfo
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

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

            modelBuilder.Entity("Identity.API.Domain.SellerInfo", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AdditionalInfo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankAccountNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContactEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

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
                            Id = new Guid("2de9034d-8b49-4ce9-8175-61e2f12e8f84"),
                            CreatedAt = new DateTime(2021, 1, 9, 18, 35, 52, 19, DateTimeKind.Utc).AddTicks(8187),
                            Email = "admin@mail.com",
                            HashedPassword = "rgD/9ec0VkDn2p62uyZ0GtBx2PNSzljck+QarzDYklw=|5AotysMDU9NsxEFX+H/EnQ==|10000",
                            Role = "admin",
                            UpdatedAt = new DateTime(2021, 1, 9, 18, 35, 52, 19, DateTimeKind.Utc).AddTicks(8540)
                        });
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

            modelBuilder.Entity("Identity.API.Domain.SellerInfo", b =>
                {
                    b.HasOne("Identity.API.Domain.User", null)
                        .WithOne("SellerInfo")
                        .HasForeignKey("Identity.API.Domain.SellerInfo", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
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
                    b.Navigation("DeliveryAddresses");

                    b.Navigation("ProfileInfo");

                    b.Navigation("SellerInfo");
                });
#pragma warning restore 612, 618
        }
    }
}
