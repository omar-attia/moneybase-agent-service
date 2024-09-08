﻿// <auto-generated />
using System;
using Agent.Dal.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Agent.Dal.Migrations
{
    [DbContext(typeof(AgentDbContext))]
    partial class AgentDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Agent.Dal.Data.Entities.Agent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsOverflow")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("MaxConcurrentChats")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<int>("SeniorityId")
                        .HasColumnType("int");

                    b.Property<int>("TeamId")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("IsActive")
                        .HasDatabaseName("IX_Agent_IsActive");

                    b.HasIndex("IsDeleted")
                        .HasDatabaseName("IX_Agent_IsDeleted");

                    b.HasIndex("IsOverflow")
                        .HasDatabaseName("IX_Agent_IsOverflow");

                    b.HasIndex("MaxConcurrentChats")
                        .HasDatabaseName("IX_Agent_MaxConcurrentChats");

                    b.HasIndex("SeniorityId");

                    b.HasIndex("TeamId");

                    b.ToTable("agent");
                });

            modelBuilder.Entity("Agent.Dal.Data.Entities.AgentAssignment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AgentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("AssignedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<Guid>("SessionId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("AgentId");

                    b.HasIndex("IsCompleted")
                        .HasDatabaseName("IX_AgentAssignment_IsCompleted");

                    b.HasIndex("SessionId")
                        .IsUnique()
                        .HasDatabaseName("IX_AgentAssignment_SessionId");

                    b.ToTable("agent-assignment");
                });

            modelBuilder.Entity("Agent.Dal.Data.Entities.PendingQueuedSession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<Guid>("SessionId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("SessionId")
                        .IsUnique()
                        .HasDatabaseName("IX_PendingQueuedSession_SessionId");

                    b.ToTable("pending-queued-session");
                });

            modelBuilder.Entity("Agent.Dal.Data.Entities.SeniorityMultiplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("MultiplierValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("SeniorityLevel")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("varchar(50)");

                    b.HasKey("Id");

                    b.ToTable("seniority-multiplier");
                });

            modelBuilder.Entity("Agent.Dal.Data.Entities.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsOfficeHours")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("varchar(100)");

                    b.Property<TimeSpan>("ShiftEndTime")
                        .HasColumnType("time(6)");

                    b.Property<TimeSpan>("ShiftStartTime")
                        .HasColumnType("time(6)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Id");

                    b.HasIndex("IsOfficeHours")
                        .HasDatabaseName("IX_Team_IsOfficeHours");

                    b.HasIndex("ShiftEndTime")
                        .HasDatabaseName("IX_Team_ShiftEndTime");

                    b.HasIndex("ShiftStartTime")
                        .HasDatabaseName("IX_Team_ShiftStartTime");

                    b.ToTable("team");
                });

            modelBuilder.Entity("Agent.Dal.Data.Entities.Agent", b =>
                {
                    b.HasOne("Agent.Dal.Data.Entities.SeniorityMultiplier", "SeniorityMultiplier")
                        .WithMany()
                        .HasForeignKey("SeniorityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Agent.Dal.Data.Entities.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SeniorityMultiplier");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("Agent.Dal.Data.Entities.AgentAssignment", b =>
                {
                    b.HasOne("Agent.Dal.Data.Entities.Agent", "Agent")
                        .WithMany("Assignments")
                        .HasForeignKey("AgentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Agent");
                });

            modelBuilder.Entity("Agent.Dal.Data.Entities.Agent", b =>
                {
                    b.Navigation("Assignments");
                });
#pragma warning restore 612, 618
        }
    }
}
