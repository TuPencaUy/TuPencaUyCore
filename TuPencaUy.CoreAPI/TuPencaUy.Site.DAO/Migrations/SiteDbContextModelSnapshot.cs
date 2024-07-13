﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TuPencaUy.Site.DAO.Models.Data;

#nullable disable

namespace TuPencaUy.Site.DAO.Migrations
{
    [DbContext(typeof(SiteDbContext))]
    partial class SiteDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("EventUser", b =>
                {
                    b.Property<int>("EventsId")
                        .HasColumnType("int");

                    b.Property<string>("UsersEmail")
                        .HasColumnType("varchar(50)");

                    b.HasKey("EventsId", "UsersEmail");

                    b.HasIndex("UsersEmail");

                    b.ToTable("EventUser", (string)null);
                });

            modelBuilder.Entity("PermissionRole", b =>
                {
                    b.Property<int>("PermissionsId")
                        .HasColumnType("int");

                    b.Property<int>("RolesId")
                        .HasColumnType("int");

                    b.HasKey("PermissionsId", "RolesId");

                    b.HasIndex("RolesId");

                    b.ToTable("PermissionRole", (string)null);
                });

            modelBuilder.Entity("TuPencaUy.Site.DAO.Models.AccessRequest", b =>
                {
                    b.Property<string>("User_email")
                        .HasColumnType("varchar(50)")
                        .HasColumnName("User_email")
                        .HasColumnOrder(0);

                    b.Property<int>("AccessStatus")
                        .HasColumnType("int")
                        .HasColumnName("AccessStatus")
                        .HasColumnOrder(1);

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("Inactive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("RequestTime")
                        .HasColumnType("datetime2")
                        .HasColumnName("RequestTime")
                        .HasColumnOrder(2);

                    b.HasKey("User_email");

                    b.ToTable("AccessRequest", (string)null);
                });

            modelBuilder.Entity("TuPencaUy.Site.DAO.Models.Bet", b =>
                {
                    b.Property<int>("Match_id")
                        .HasColumnType("int")
                        .HasColumnName("Match_id")
                        .HasColumnOrder(1);

                    b.Property<int>("Event_id")
                        .HasColumnType("int")
                        .HasColumnName("Event_id")
                        .HasColumnOrder(0);

                    b.Property<string>("User_email")
                        .HasColumnType("varchar")
                        .HasColumnName("User_email")
                        .HasColumnOrder(2);

                    b.Property<int?>("Points")
                        .HasColumnType("int")
                        .HasColumnName("Points")
                        .HasColumnOrder(5);

                    b.Property<int>("ScoreFirstTeam")
                        .HasColumnType("int")
                        .HasColumnName("ScoreFirstTeam")
                        .HasColumnOrder(3);

                    b.Property<int>("ScoreSecondTeam")
                        .HasColumnType("int")
                        .HasColumnName("ScoreSecondTeam")
                        .HasColumnOrder(4);

                    b.HasKey("Match_id", "Event_id", "User_email");

                    b.HasIndex("Event_id");

                    b.HasIndex("User_email");

                    b.ToTable("Bet", (string)null);
                });

            modelBuilder.Entity("TuPencaUy.Site.DAO.Models.Event", b =>
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

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<decimal>("PrizePercentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("RefEvent")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("DateTime")
                        .HasColumnName("StartDate")
                        .HasColumnOrder(2);

                    b.Property<int?>("TeamType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Event", (string)null);
                });

            modelBuilder.Entity("TuPencaUy.Site.DAO.Models.Match", b =>
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

                    b.Property<int>("RefMatch")
                        .HasColumnType("int");

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

            modelBuilder.Entity("TuPencaUy.Site.DAO.Models.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id")
                        .HasColumnOrder(0);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)")
                        .HasColumnName("Amount")
                        .HasColumnOrder(3);

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Event_id")
                        .HasColumnType("int")
                        .HasColumnName("Event_id")
                        .HasColumnOrder(1);

                    b.Property<bool>("Inactive")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("LastModificationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("TransactionID")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar")
                        .HasColumnName("TransactionID")
                        .HasColumnOrder(4);

                    b.Property<string>("User_email")
                        .IsRequired()
                        .HasColumnType("varchar")
                        .HasColumnName("User_email")
                        .HasColumnOrder(2);

                    b.HasKey("Id");

                    b.HasIndex("Event_id");

                    b.HasIndex("User_email");

                    b.ToTable("Payment", (string)null);
                });

            modelBuilder.Entity("TuPencaUy.Site.DAO.Models.Permission", b =>
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

            modelBuilder.Entity("TuPencaUy.Site.DAO.Models.Role", b =>
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

            modelBuilder.Entity("TuPencaUy.Site.DAO.Models.Sport", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("Id")
                        .HasColumnOrder(0);

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("EventId")
                        .HasColumnType("int");

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

                    b.Property<int>("RefSport")
                        .HasColumnType("int");

                    b.Property<bool>("Tie")
                        .HasColumnType("bit")
                        .HasColumnName("Tie")
                        .HasColumnOrder(2);

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.ToTable("Sport", (string)null);
                });

            modelBuilder.Entity("TuPencaUy.Site.DAO.Models.Team", b =>
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

                    b.Property<int>("RefTeam")
                        .HasColumnType("int");

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

            modelBuilder.Entity("TuPencaUy.Site.DAO.Models.User", b =>
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

                    b.Property<string>("PaypalEmail")
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("paypalEmail")
                        .HasColumnOrder(5);

                    b.Property<int?>("roleId")
                        .HasColumnType("int")
                        .HasColumnName("RoleId")
                        .HasColumnOrder(4);

                    b.HasKey("Email");

                    b.HasIndex("roleId");

                    b.ToTable("User", (string)null);
                });

            modelBuilder.Entity("EventUser", b =>
                {
                    b.HasOne("TuPencaUy.Site.DAO.Models.Event", null)
                        .WithMany()
                        .HasForeignKey("EventsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TuPencaUy.Site.DAO.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersEmail")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PermissionRole", b =>
                {
                    b.HasOne("TuPencaUy.Site.DAO.Models.Permission", null)
                        .WithMany()
                        .HasForeignKey("PermissionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TuPencaUy.Site.DAO.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("TuPencaUy.Site.DAO.Models.AccessRequest", b =>
                {
                    b.HasOne("TuPencaUy.Site.DAO.Models.User", "User")
                        .WithOne("AccessRequest")
                        .HasForeignKey("TuPencaUy.Site.DAO.Models.AccessRequest", "User_email")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("TuPencaUy.Site.DAO.Models.Bet", b =>
                {
                    b.HasOne("TuPencaUy.Site.DAO.Models.Event", "Event")
                        .WithMany("Bets")
                        .HasForeignKey("Event_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TuPencaUy.Site.DAO.Models.Match", "Match")
                        .WithMany("Bets")
                        .HasForeignKey("Match_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TuPencaUy.Site.DAO.Models.User", "User")
                        .WithMany("Bets")
                        .HasForeignKey("User_email")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Match");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TuPencaUy.Site.DAO.Models.Match", b =>
                {
                    b.HasOne("TuPencaUy.Site.DAO.Models.Event", "Event")
                        .WithMany("Matches")
                        .HasForeignKey("Event_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TuPencaUy.Site.DAO.Models.Team", "FirstTeam")
                        .WithMany()
                        .HasForeignKey("FirstTeam_id");

                    b.HasOne("TuPencaUy.Site.DAO.Models.Team", "SecondTeam")
                        .WithMany()
                        .HasForeignKey("SecondTeam_id");

                    b.HasOne("TuPencaUy.Site.DAO.Models.Sport", "Sport")
                        .WithMany()
                        .HasForeignKey("Sport_id");

                    b.Navigation("Event");

                    b.Navigation("FirstTeam");

                    b.Navigation("SecondTeam");

                    b.Navigation("Sport");
                });

            modelBuilder.Entity("TuPencaUy.Site.DAO.Models.Payment", b =>
                {
                    b.HasOne("TuPencaUy.Site.DAO.Models.Event", "Event")
                        .WithMany()
                        .HasForeignKey("Event_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TuPencaUy.Site.DAO.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("User_email")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("User");
                });

            modelBuilder.Entity("TuPencaUy.Site.DAO.Models.Team", b =>
                {
                    b.HasOne("TuPencaUy.Site.DAO.Models.Sport", "Sport")
                        .WithMany()
                        .HasForeignKey("Sport_id");

                    b.Navigation("Sport");
                });

            modelBuilder.Entity("TuPencaUy.Site.DAO.Models.User", b =>
                {
                    b.HasOne("TuPencaUy.Site.DAO.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("roleId");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("TuPencaUy.Site.DAO.Models.Event", b =>
                {
                    b.Navigation("Bets");

                    b.Navigation("Matches");
                });

            modelBuilder.Entity("TuPencaUy.Site.DAO.Models.Match", b =>
                {
                    b.Navigation("Bets");
                });

            modelBuilder.Entity("TuPencaUy.Site.DAO.Models.User", b =>
                {
                    b.Navigation("AccessRequest")
                        .IsRequired();

                    b.Navigation("Bets");
                });
#pragma warning restore 612, 618
        }
    }
}
