﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TennisTournament.Infrastructure.Data;

#nullable disable

namespace TennisTournament.Infrastructure.Migrations
{
    [DbContext(typeof(TournamentDbContext))]
    partial class TournamentDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TennisTournament.Domain.Entities.Match", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("MatchDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("Player1Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Player2Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("ResultId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Round")
                        .HasColumnType("int");

                    b.Property<Guid>("TournamentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("WinnerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("Player1Id");

                    b.HasIndex("Player2Id");

                    b.HasIndex("ResultId");

                    b.HasIndex("TournamentId");

                    b.HasIndex("WinnerId");

                    b.ToTable("Matches");
                });

            modelBuilder.Entity("TennisTournament.Domain.Entities.Player", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("PlayerType")
                        .HasColumnType("int");

                    b.Property<int>("SkillLevel")
                        .HasColumnType("int");

                    b.Property<Guid?>("TournamentId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TournamentId");

                    b.ToTable("Players");

                    b.HasDiscriminator<int>("PlayerType");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("TennisTournament.Domain.Entities.Result", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("CompletionDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsFinalized")
                        .HasColumnType("bit");

                    b.Property<Guid>("TournamentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("WinnerId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("TournamentId");

                    b.HasIndex("WinnerId");

                    b.ToTable("Results");
                });

            modelBuilder.Entity("TennisTournament.Domain.Entities.Tournament", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Tournaments");
                });

            modelBuilder.Entity("TennisTournament.Domain.Entities.FemalePlayer", b =>
                {
                    b.HasBaseType("TennisTournament.Domain.Entities.Player");

                    b.Property<int>("ReactionTime")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("TennisTournament.Domain.Entities.MalePlayer", b =>
                {
                    b.HasBaseType("TennisTournament.Domain.Entities.Player");

                    b.Property<int>("Speed")
                        .HasColumnType("int");

                    b.Property<int>("Strength")
                        .HasColumnType("int");

                    b.HasDiscriminator().HasValue(0);
                });

            modelBuilder.Entity("TennisTournament.Domain.Entities.Match", b =>
                {
                    b.HasOne("TennisTournament.Domain.Entities.Player", "Player1")
                        .WithMany()
                        .HasForeignKey("Player1Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TennisTournament.Domain.Entities.Player", "Player2")
                        .WithMany()
                        .HasForeignKey("Player2Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("TennisTournament.Domain.Entities.Result", null)
                        .WithMany("Matches")
                        .HasForeignKey("ResultId");

                    b.HasOne("TennisTournament.Domain.Entities.Tournament", "Tournament")
                        .WithMany("Matches")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TennisTournament.Domain.Entities.Player", "Winner")
                        .WithMany()
                        .HasForeignKey("WinnerId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("Player1");

                    b.Navigation("Player2");

                    b.Navigation("Tournament");

                    b.Navigation("Winner");
                });

            modelBuilder.Entity("TennisTournament.Domain.Entities.Player", b =>
                {
                    b.HasOne("TennisTournament.Domain.Entities.Tournament", null)
                        .WithMany("Players")
                        .HasForeignKey("TournamentId");
                });

            modelBuilder.Entity("TennisTournament.Domain.Entities.Result", b =>
                {
                    b.HasOne("TennisTournament.Domain.Entities.Tournament", "Tournament")
                        .WithMany()
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("TennisTournament.Domain.Entities.Player", "Winner")
                        .WithMany()
                        .HasForeignKey("WinnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tournament");

                    b.Navigation("Winner");
                });

            modelBuilder.Entity("TennisTournament.Domain.Entities.Result", b =>
                {
                    b.Navigation("Matches");
                });

            modelBuilder.Entity("TennisTournament.Domain.Entities.Tournament", b =>
                {
                    b.Navigation("Matches");

                    b.Navigation("Players");
                });
#pragma warning restore 612, 618
        }
    }
}
