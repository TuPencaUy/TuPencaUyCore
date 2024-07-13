﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TuPencaUy.Platform.DAO.Models.Data;

#nullable disable

namespace TuPencaUy.Platform.DAO.Migrations
{
    [DbContext(typeof(PlatformDbContext))]
    [Migration("20240713015851_adding-payout")]
    partial class addingpayout
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EventSport", b =>
                {
                    b.Property<int>("EventsId")
                        .HasColumnType("int");

                    b.Property<int>("SportsId")
                        .HasColumnType("int");

                    b.HasKey("EventsId", "SportsId");

                    b.HasIndex("SportsId");

                    b.ToTable("EventSport");
                });

            modelBuilder.Entity("PermissionRole", b =>
                {
                    b.Property<int>("PermissionsId")
                        .HasColumnType("int");

                    b.Property<int>("RolesId")
                        .HasColumnType("int");

                    b.HasKey("PermissionsId", "RolesId");

                    b.HasIndex("RolesId");

                    b.ToTable("PermissionRole");
                });

            modelBuilder.Entity("TuPencaUy.Platform.DAO.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id")
                        .HasColumnOrder(0);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double?>("Comission")
                        .HasColumnType("float")
                        .HasColumnName("Comission")
                        .HasColumnOrder(4);

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("DateTime")
                        .HasColumnName("EndDate")
                        .HasColumnOrder(3);

                    b.Property<bool>("Finished")
                        .HasColumnType("bit")
                        .HasColumnName("Finished")
                        .HasColumnOrder(6);

                    b.Property<bool>("Inactive")
                        .HasColumnType("bit");

                    b.Property<bool>("Instantiable")
                        .HasColumnType("bit")
                        .HasColumnName("Instantiable")
                        .HasColumnOrder(5);

                    b.Property<DateTime?>("LastModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar")
                        .HasColumnName("Name")
                        .HasColumnOrder(1);

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("DateTime")
                        .HasColumnName("StartDate")
                        .HasColumnOrder(2);

                    b.Property<int?>("TeamType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Event", (string)null);
                });

            modelBuilder.Entity("TuPencaUy.Platform.DAO.Models.Match", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id")
                        .HasColumnOrder(0);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("Date")
                        .HasColumnType("datetime2")
                        .HasColumnName("Date")
                        .HasColumnOrder(5);

                    b.Property<int>("Event_id")
                        .HasColumnType("int")
                        .HasColumnName("Event_id")
                        .HasColumnOrder(7);

                    b.Property<bool>("Finished")
                        .HasColumnType("bit")
                        .HasColumnName("Finished")
                        .HasColumnOrder(8);

                    b.Property<int?>("FirstTeamScore")
                        .HasColumnType("int")
                        .HasColumnName("FirstTeamScore")
                        .HasColumnOrder(3);

                    b.Property<int?>("FirstTeam_id")
                        .HasColumnType("int")
                        .HasColumnName("FirstTeam")
                        .HasColumnOrder(1);

                    b.Property<bool>("Inactive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("SecondTeamScore")
                        .HasColumnType("int")
                        .HasColumnName("SecondTeamScore")
                        .HasColumnOrder(4);

                    b.Property<int?>("SecondTeam_id")
                        .HasColumnType("int")
                        .HasColumnName("SecondTeam")
                        .HasColumnOrder(2);

                    b.Property<int?>("Sport_id")
                        .HasColumnType("int")
                        .HasColumnName("Sport_id")
                        .HasColumnOrder(6);

                    b.HasKey("Id");

                    b.HasIndex("Event_id");

                    b.HasIndex("FirstTeam_id");

                    b.HasIndex("SecondTeam_id");

                    b.HasIndex("Sport_id");

                    b.ToTable("Match", (string)null);
                });

            modelBuilder.Entity("TuPencaUy.Platform.DAO.Models.Payout", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id")
                        .HasColumnOrder(0);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal")
                        .HasColumnName("Amount")
                        .HasColumnOrder(3);

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Event_id")
                        .HasColumnType("int")
                        .HasColumnName("Event_id")
                        .HasColumnOrder(5);

                    b.Property<bool>("Inactive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("PaypalEmail")
                        .IsRequired()
                        .HasColumnType("varchar")
                        .HasColumnName("PaypalEmail")
                        .HasColumnOrder(2);

                    b.Property<int>("Site_id")
                        .HasColumnType("int")
                        .HasColumnName("Site_id")
                        .HasColumnOrder(1);

                    b.Property<string>("TransactionID")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar")
                        .HasColumnName("TransactionID")
                        .HasColumnOrder(4);

                    b.HasKey("Id");

                    b.HasIndex("Event_id");

                    b.ToTable("Payout", (string)null);
                });

            modelBuilder.Entity("TuPencaUy.Platform.DAO.Models.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id")
                        .HasColumnOrder(0);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar")
                        .HasColumnName("Description")
                        .HasColumnOrder(2);

                    b.Property<bool>("Inactive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar")
                        .HasColumnName("Name")
                        .HasColumnOrder(1);

                    b.HasKey("Id");

                    b.ToTable("Permission", (string)null);
                });

            modelBuilder.Entity("TuPencaUy.Platform.DAO.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id")
                        .HasColumnOrder(0);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Inactive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar")
                        .HasColumnName("Name")
                        .HasColumnOrder(1);

                    b.HasKey("Id");

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("TuPencaUy.Platform.DAO.Models.Site", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id")
                        .HasColumnOrder(0);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AccessType")
                        .HasColumnType("int");

                    b.Property<int?>("Color")
                        .HasColumnType("int");

                    b.Property<string>("ConnectionString")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("varchar")
                        .HasColumnName("ConnectionString")
                        .HasColumnOrder(3);

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Domain")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar")
                        .HasColumnName("Domain")
                        .HasColumnOrder(2);

                    b.Property<bool>("Inactive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar")
                        .HasColumnName("Name")
                        .HasColumnOrder(1);

                    b.Property<string>("PaypalEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserEmail")
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("UserEmail");

                    b.ToTable("Site", (string)null);
                });

            modelBuilder.Entity("TuPencaUy.Platform.DAO.Models.Sport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id")
                        .HasColumnOrder(0);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("ExactPoints")
                        .HasColumnType("int")
                        .HasColumnName("ExactPoints")
                        .HasColumnOrder(3);

                    b.Property<bool>("Inactive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar")
                        .HasColumnName("Name")
                        .HasColumnOrder(1);

                    b.Property<int?>("PartialPoints")
                        .HasColumnType("int")
                        .HasColumnName("PartialPoints")
                        .HasColumnOrder(4);

                    b.Property<bool>("Tie")
                        .HasColumnType("bit")
                        .HasColumnName("Tie")
                        .HasColumnOrder(2);

                    b.HasKey("Id");

                    b.ToTable("Sport", (string)null);
                });

            modelBuilder.Entity("TuPencaUy.Platform.DAO.Models.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id")
                        .HasColumnOrder(0);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Inactive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("Logo")
                        .HasColumnType("image")
                        .HasColumnName("Logo")
                        .HasColumnOrder(2);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar")
                        .HasColumnName("Name")
                        .HasColumnOrder(1);

                    b.Property<int?>("Sport_id")
                        .HasColumnType("int")
                        .HasColumnName("Sport_id")
                        .HasColumnOrder(3);

                    b.Property<int?>("TeamType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Sport_id");

                    b.ToTable("Team", (string)null);
                });

            modelBuilder.Entity("TuPencaUy.Platform.DAO.Models.User", b =>
                {
                    b.Property<string>("Email")
                        .HasMaxLength(50)
                        .HasColumnType("varchar")
                        .HasColumnName("Email")
                        .HasColumnOrder(2);

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id")
                        .HasColumnOrder(0);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Inactive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar")
                        .HasColumnName("Name")
                        .HasColumnOrder(1);

                    b.Property<string>("Password")
                        .HasMaxLength(100)
                        .HasColumnType("varchar")
                        .HasColumnName("Password")
                        .HasColumnOrder(3);

                    b.Property<int?>("roleId")
                        .HasColumnType("int")
                        .HasColumnName("RoleId")
                        .HasColumnOrder(4);

                    b.HasKey("Email");

                    b.HasIndex("roleId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("EventSport", b =>
                {
                    b.HasOne("TuPencaUy.Platform.DAO.Models.Event", null)
                        .WithMany()
                        .HasForeignKey("EventsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TuPencaUy.Platform.DAO.Models.Sport", null)
                        .WithMany()
                        .HasForeignKey("SportsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PermissionRole", b =>
                {
                    b.HasOne("TuPencaUy.Platform.DAO.Models.Permission", null)
                        .WithMany()
                        .HasForeignKey("PermissionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TuPencaUy.Platform.DAO.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TuPencaUy.Platform.DAO.Models.Match", b =>
                {
                    b.HasOne("TuPencaUy.Platform.DAO.Models.Event", "Event")
                        .WithMany("Matches")
                        .HasForeignKey("Event_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TuPencaUy.Platform.DAO.Models.Team", "FirstTeam")
                        .WithMany()
                        .HasForeignKey("FirstTeam_id");

                    b.HasOne("TuPencaUy.Platform.DAO.Models.Team", "SecondTeam")
                        .WithMany()
                        .HasForeignKey("SecondTeam_id");

                    b.HasOne("TuPencaUy.Platform.DAO.Models.Sport", "Sport")
                        .WithMany("Matches")
                        .HasForeignKey("Sport_id");

                    b.Navigation("Event");

                    b.Navigation("FirstTeam");

                    b.Navigation("SecondTeam");

                    b.Navigation("Sport");
                });

            modelBuilder.Entity("TuPencaUy.Platform.DAO.Models.Payout", b =>
                {
                    b.HasOne("TuPencaUy.Platform.DAO.Models.Event", "Event")
                        .WithMany()
                        .HasForeignKey("Event_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");
                });

            modelBuilder.Entity("TuPencaUy.Platform.DAO.Models.Site", b =>
                {
                    b.HasOne("TuPencaUy.Platform.DAO.Models.User", null)
                        .WithMany("Sites")
                        .HasForeignKey("UserEmail");
                });

            modelBuilder.Entity("TuPencaUy.Platform.DAO.Models.Team", b =>
                {
                    b.HasOne("TuPencaUy.Platform.DAO.Models.Sport", "Sport")
                        .WithMany("Teams")
                        .HasForeignKey("Sport_id");

                    b.Navigation("Sport");
                });

            modelBuilder.Entity("TuPencaUy.Platform.DAO.Models.User", b =>
                {
                    b.HasOne("TuPencaUy.Platform.DAO.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("roleId");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("TuPencaUy.Platform.DAO.Models.Event", b =>
                {
                    b.Navigation("Matches");
                });

            modelBuilder.Entity("TuPencaUy.Platform.DAO.Models.Sport", b =>
                {
                    b.Navigation("Matches");

                    b.Navigation("Teams");
                });

            modelBuilder.Entity("TuPencaUy.Platform.DAO.Models.User", b =>
                {
                    b.Navigation("Sites");
                });
#pragma warning restore 612, 618
        }
    }
}
