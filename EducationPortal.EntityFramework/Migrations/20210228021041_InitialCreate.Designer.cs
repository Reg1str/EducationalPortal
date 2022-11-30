﻿// <auto-generated />
using System;
using EducationPortal.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace EducationPortal.EntityFramework.Migrations
{
    [DbContext(typeof(EfContext))]
    [Migration("20210228021041_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EducationPortal.Domain.Core.Entities.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Author");
                });

            modelBuilder.Entity("EducationPortal.Domain.Core.Entities.Course", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Course");
                });

            modelBuilder.Entity("EducationPortal.Domain.Core.Entities.Material", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Material");
                });

            modelBuilder.Entity("EducationPortal.Domain.Core.Entities.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MaxLevel")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("ProgressPenalty")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Skill");
                });

            modelBuilder.Entity("EducationPortal.Domain.Core.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<string>("UserType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("EducationPortal.Domain.Core.Mappings.AuthorPrintedMaterial", b =>
                {
                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<int>("PrintedMaterialId")
                        .HasColumnType("int");

                    b.HasKey("AuthorId", "PrintedMaterialId");

                    b.HasIndex("PrintedMaterialId");

                    b.ToTable("AuthorPrintedMaterial");
                });

            modelBuilder.Entity("EducationPortal.Domain.Core.Mappings.CourseMaterial", b =>
                {
                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("MaterialId")
                        .HasColumnType("int");

                    b.HasKey("CourseId", "MaterialId");

                    b.HasIndex("MaterialId");

                    b.ToTable("CourseMaterial");
                });

            modelBuilder.Entity("EducationPortal.Domain.Core.Mappings.SkillCourse", b =>
                {
                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("SkillId")
                        .HasColumnType("int");

                    b.HasKey("CourseId", "SkillId");

                    b.HasIndex("SkillId");

                    b.ToTable("SkillCourse");
                });

            modelBuilder.Entity("EducationPortal.Domain.Core.Mappings.UserCourseEnrollment", b =>
                {
                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("ProgressPercentage")
                        .HasColumnType("int");

                    b.HasKey("CourseId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserCourseEnrollment");
                });

            modelBuilder.Entity("EducationPortal.Domain.Core.Mappings.UserCourseOwner", b =>
                {
                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("CourseId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserCourseOwner");
                });

            modelBuilder.Entity("EducationPortal.Domain.Core.Mappings.UserSkills", b =>
                {
                    b.Property<int>("SkillId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("CurrentLevel")
                        .HasColumnType("int");

                    b.Property<int>("CurrentProgress")
                        .HasColumnType("int");

                    b.HasKey("SkillId", "UserId");

                    b.HasIndex("UserId");

                    b.ToTable("UserSkills");
                });

            modelBuilder.Entity("EducationPortal.Domain.Core.Entities.Article", b =>
                {
                    b.HasBaseType("EducationPortal.Domain.Core.Entities.Material");

                    b.Property<DateTime>("PublishedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("SourceUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Article");
                });

            modelBuilder.Entity("EducationPortal.Domain.Core.Entities.PrintedMaterial", b =>
                {
                    b.HasBaseType("EducationPortal.Domain.Core.Entities.Material");

                    b.Property<int>("PagesCount")
                        .HasColumnType("int");

                    b.ToTable("PrintedMaterial");
                });

            modelBuilder.Entity("EducationPortal.Domain.Core.Entities.Video", b =>
                {
                    b.HasBaseType("EducationPortal.Domain.Core.Entities.Material");

                    b.Property<int>("Length")
                        .HasColumnType("int");

                    b.Property<int>("Quality")
                        .HasColumnType("int");

                    b.ToTable("Video");
                });

            modelBuilder.Entity("EducationPortal.Domain.Core.Mappings.AuthorPrintedMaterial", b =>
                {
                    b.HasOne("EducationPortal.Domain.Core.Entities.Author", "Author")
                        .WithMany("AuthorPrintedMaterials")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationPortal.Domain.Core.Entities.PrintedMaterial", "PrintedMaterial")
                        .WithMany("AuthorPrintedMaterials")
                        .HasForeignKey("PrintedMaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("PrintedMaterial");
                });

            modelBuilder.Entity("EducationPortal.Domain.Core.Mappings.CourseMaterial", b =>
                {
                    b.HasOne("EducationPortal.Domain.Core.Entities.Course", "Course")
                        .WithMany("CourseMaterials")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationPortal.Domain.Core.Entities.Material", "Material")
                        .WithMany("CourseMaterials")
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Material");
                });

            modelBuilder.Entity("EducationPortal.Domain.Core.Mappings.SkillCourse", b =>
                {
                    b.HasOne("EducationPortal.Domain.Core.Entities.Course", "Course")
                        .WithMany("SkillCourses")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationPortal.Domain.Core.Entities.Skill", "Skill")
                        .WithMany("SkillCourses")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Skill");
                });

            modelBuilder.Entity("EducationPortal.Domain.Core.Mappings.UserCourseEnrollment", b =>
                {
                    b.HasOne("EducationPortal.Domain.Core.Entities.Course", "Course")
                        .WithMany("UserCourseEnrollments")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationPortal.Domain.Core.Entities.User", "User")
                        .WithMany("UserCourseEnrollments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EducationPortal.Domain.Core.Mappings.UserCourseOwner", b =>
                {
                    b.HasOne("EducationPortal.Domain.Core.Entities.Course", "Course")
                        .WithMany("UserCourseOwners")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationPortal.Domain.Core.Entities.User", "User")
                        .WithMany("UserCourseOwners")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EducationPortal.Domain.Core.Mappings.UserSkills", b =>
                {
                    b.HasOne("EducationPortal.Domain.Core.Entities.Skill", "Skill")
                        .WithMany("UserSkills")
                        .HasForeignKey("SkillId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EducationPortal.Domain.Core.Entities.User", "User")
                        .WithMany("UserSkills")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Skill");

                    b.Navigation("User");
                });

            modelBuilder.Entity("EducationPortal.Domain.Core.Entities.Article", b =>
                {
                    b.HasOne("EducationPortal.Domain.Core.Entities.Material", null)
                        .WithOne()
                        .HasForeignKey("EducationPortal.Domain.Core.Entities.Article", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EducationPortal.Domain.Core.Entities.PrintedMaterial", b =>
                {
                    b.HasOne("EducationPortal.Domain.Core.Entities.Material", null)
                        .WithOne()
                        .HasForeignKey("EducationPortal.Domain.Core.Entities.PrintedMaterial", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EducationPortal.Domain.Core.Entities.Video", b =>
                {
                    b.HasOne("EducationPortal.Domain.Core.Entities.Material", null)
                        .WithOne()
                        .HasForeignKey("EducationPortal.Domain.Core.Entities.Video", "Id")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EducationPortal.Domain.Core.Entities.Author", b =>
                {
                    b.Navigation("AuthorPrintedMaterials");
                });

            modelBuilder.Entity("EducationPortal.Domain.Core.Entities.Course", b =>
                {
                    b.Navigation("CourseMaterials");

                    b.Navigation("SkillCourses");

                    b.Navigation("UserCourseEnrollments");

                    b.Navigation("UserCourseOwners");
                });

            modelBuilder.Entity("EducationPortal.Domain.Core.Entities.Material", b =>
                {
                    b.Navigation("CourseMaterials");
                });

            modelBuilder.Entity("EducationPortal.Domain.Core.Entities.Skill", b =>
                {
                    b.Navigation("SkillCourses");

                    b.Navigation("UserSkills");
                });

            modelBuilder.Entity("EducationPortal.Domain.Core.Entities.User", b =>
                {
                    b.Navigation("UserCourseEnrollments");

                    b.Navigation("UserCourseOwners");

                    b.Navigation("UserSkills");
                });

            modelBuilder.Entity("EducationPortal.Domain.Core.Entities.PrintedMaterial", b =>
                {
                    b.Navigation("AuthorPrintedMaterials");
                });
#pragma warning restore 612, 618
        }
    }
}
