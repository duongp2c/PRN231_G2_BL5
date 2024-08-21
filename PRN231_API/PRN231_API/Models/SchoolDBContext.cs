using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PRN231_API.Models
{
    public partial class SchoolDBContext : DbContext
    {
        public SchoolDBContext()
        {
        }

        public SchoolDBContext(DbContextOptions<SchoolDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Evaluation> Evaluations { get; set; } = null!;
        public virtual DbSet<GradeType> GradeTypes { get; set; } = null!;
        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<StudentDetail> StudentDetails { get; set; } = null!;
        public virtual DbSet<StudentSubject> StudentSubjects { get; set; } = null!;
        public virtual DbSet<Subject> Subjects { get; set; } = null!;
        public virtual DbSet<Teacher> Teachers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {


        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.ActiveCode).HasMaxLength(100);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<Evaluation>(entity =>
            {
                entity.ToTable("Evaluation");

                entity.Property(e => e.AdditionExplanation).HasMaxLength(255);

                entity.Property(e => e.Grade).HasColumnType("decimal(4, 2)");

                entity.HasOne(d => d.GradeType)
                    .WithMany(p => p.Evaluations)
                    .HasForeignKey(d => d.GradeTypeId)
                    .HasConstraintName("FK__Evaluatio__Grade__3D5E1FD2");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Evaluations)
                    .HasForeignKey(d => d.StudentId)
                    .HasConstraintName("FK__Evaluatio__Stude__3B75D760");


                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.Evaluations)
                    .HasForeignKey(d => d.SubjectId)
                    .HasConstraintName("FK__Evaluatio__Subje__3C69FB99");
            });

            modelBuilder.Entity<GradeType>(entity =>
            {
                entity.ToTable("GradeType");
                entity.Property(e => e.GradeTypeName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.GradeTypeWeight).HasColumnType("decimal(5, 2)");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");
                entity.HasIndex(e => e.AccountId, "UQ__Student__349DA5A757481177")
                    .IsUnique();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Account)
                    .WithOne(p => p.Student)
                    .HasForeignKey<Student>(d => d.AccountId)
                    .HasConstraintName("FK__Student__Account__286302EC");
            });

            modelBuilder.Entity<StudentDetail>(entity =>
            {
                entity.HasKey(e => e.StudentDetailsId)
                    .HasName("PK__StudentD__3963A24F011D4767");

                entity.HasIndex(e => e.StudentId, "UQ__StudentD__32C52B98F9EF5F75")
                    .IsUnique();

                entity.Property(e => e.AdditionalInformation).HasMaxLength(255);

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.Image).HasMaxLength(255);

                entity.Property(e => e.Phone).HasMaxLength(15);

                entity.HasOne(d => d.Student)
                    .WithOne(p => p.StudentDetail)
                    .HasForeignKey<StudentDetail>(d => d.StudentId)
                    .HasConstraintName("FK__StudentDe__Stude__2C3393D0");
            });

            modelBuilder.Entity<StudentSubject>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.SubjectId })
                    .HasName("PK__StudentS__A80491A36F94AA95");

                entity.ToTable("StudentSubject");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentSubjects)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StudentSu__Stude__37A5467C");

                entity.HasOne(d => d.Subject)
                    .WithMany(p => p.StudentSubjects)
                    .HasForeignKey(d => d.SubjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__StudentSu__Subje__38996AB5");
            });

            modelBuilder.Entity<Subject>(entity =>
            {
                entity.ToTable("Subject");
                entity.Property(e => e.SubjectName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Teacher)
                    .WithMany(p => p.Subjects)
                    .HasForeignKey(d => d.TeacherId)
                    .HasConstraintName("FK__Subject__Teacher__3B75D760");
            });

            modelBuilder.Entity<Teacher>(entity =>
            {
                entity.ToTable("Teacher");
                entity.HasIndex(e => e.AccountId, "UQ__Teacher__349DA5A762308E4C")
                    .IsUnique();

                entity.Property(e => e.TeacherName).HasMaxLength(100);

                entity.HasOne(d => d.Account)
                    .WithOne(p => p.Teacher)
                    .HasForeignKey<Teacher>(d => d.AccountId)
                    .HasConstraintName("FK__Teacher__Account__3C69FB99");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
