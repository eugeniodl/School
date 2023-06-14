﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SchoolAPI.Data;

#nullable disable

namespace SchoolAPI.Migrations
{
    [DbContext(typeof(SchoolContext))]
    partial class SchoolContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SchoolAPI.Models.Grade", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<string>("GradeName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Section")
                        .IsRequired()
                        .HasColumnType("nvarchar(1)");

                    b.HasKey("Id");

                    b.ToTable("Grades");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            GradeName = "Primero",
                            Section = "A"
                        },
                        new
                        {
                            Id = 2,
                            GradeName = "Primero",
                            Section = "B"
                        });
                });

            modelBuilder.Entity("SchoolAPI.Models.Student", b =>
                {
                    b.Property<int>("StudentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("StudentId"));

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<int>("GradeId")
                        .HasColumnType("int");

                    b.Property<string>("StudentName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("StudentId");

                    b.HasIndex("GradeId");

                    b.ToTable("Students");

                    b.HasData(
                        new
                        {
                            StudentId = 1,
                            DateOfBirth = new DateTime(2018, 12, 23, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            GradeId = 1,
                            StudentName = "Marcela Padilla"
                        },
                        new
                        {
                            StudentId = 2,
                            DateOfBirth = new DateTime(2019, 7, 25, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            GradeId = 2,
                            StudentName = "Leonardo Amador"
                        },
                        new
                        {
                            StudentId = 3,
                            DateOfBirth = new DateTime(2017, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            GradeId = 1,
                            StudentName = "Karla Reyes"
                        });
                });

            modelBuilder.Entity("SchoolAPI.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Password = "1234.",
                            Role = "Administrator",
                            UserName = "administrator"
                        },
                        new
                        {
                            Id = 2,
                            Password = "1234.",
                            Role = "Teacher",
                            UserName = "teacher"
                        });
                });

            modelBuilder.Entity("SchoolAPI.Models.Student", b =>
                {
                    b.HasOne("SchoolAPI.Models.Grade", "Grade")
                        .WithMany()
                        .HasForeignKey("GradeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Grade");
                });
#pragma warning restore 612, 618
        }
    }
}
