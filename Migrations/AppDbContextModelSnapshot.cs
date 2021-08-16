﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using iread_school_ms.DataAccess.Data;

namespace iread_school_ms.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("iread_school_ms.DataAccess.Data.Entity.Class", b =>
                {
                    b.Property<int>("ClassId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("SchoolId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ClassId");

                    b.HasIndex("SchoolId");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("iread_school_ms.DataAccess.Data.Entity.ClassMember", b =>
                {
                    b.Property<int>("ClassMemberId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ClassId")
                        .HasColumnType("int");

                    b.Property<string>("ClassMembershipType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("MemberId")
                        .HasColumnType("text");

                    b.HasKey("ClassMemberId");

                    b.HasIndex("ClassId");

                    b.ToTable("ClassMembers");
                });

            modelBuilder.Entity("iread_school_ms.DataAccess.Data.Entity.School", b =>
                {
                    b.Property<int>("SchoolId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("SchoolId");

                    b.ToTable("Schools");
                });

            modelBuilder.Entity("iread_school_ms.DataAccess.Data.Entity.SchoolManager", b =>
                {
                    b.Property<int>("SchoolManagerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ManagerId")
                        .HasColumnType("int");

                    b.Property<int>("SchoolId")
                        .HasColumnType("int");

                    b.HasKey("SchoolManagerId");

                    b.HasIndex("SchoolId");

                    b.ToTable("SchoolManagers");
                });

            modelBuilder.Entity("iread_school_ms.DataAccess.Data.Entity.Class", b =>
                {
                    b.HasOne("iread_school_ms.DataAccess.Data.Entity.School", "School")
                        .WithMany("Classes")
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("School");
                });

            modelBuilder.Entity("iread_school_ms.DataAccess.Data.Entity.ClassMember", b =>
                {
                    b.HasOne("iread_school_ms.DataAccess.Data.Entity.Class", "Class")
                        .WithMany("Members")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Class");
                });

            modelBuilder.Entity("iread_school_ms.DataAccess.Data.Entity.SchoolManager", b =>
                {
                    b.HasOne("iread_school_ms.DataAccess.Data.Entity.School", "School")
                        .WithMany("Managers")
                        .HasForeignKey("SchoolId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("School");
                });

            modelBuilder.Entity("iread_school_ms.DataAccess.Data.Entity.Class", b =>
                {
                    b.Navigation("Members");
                });

            modelBuilder.Entity("iread_school_ms.DataAccess.Data.Entity.School", b =>
                {
                    b.Navigation("Classes");

                    b.Navigation("Managers");
                });
#pragma warning restore 612, 618
        }
    }
}
