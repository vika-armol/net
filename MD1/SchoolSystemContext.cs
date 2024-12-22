using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MD1.Person;

namespace MD1
{
    public class SchoolSystemContext : DbContext
    {
        //Dbset īpašības
        public DbSet<Teacher> Teacher { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Assignement> Assignement { get; set; }
        public DbSet<Submission> Submission { get; set; }
        public DbSet<Student> Student { get; set; }

        //Bezparametriskais kontrukstors
        public SchoolSystemContext() { }

        //Konstruktors
        public SchoolSystemContext(DbContextOptions<SchoolSystemContext> options) : base(options) { }

        //Konfigurē datubāzes savienojumu izmantojot Appsettings.json failu (10
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var basePath = AppContext.BaseDirectory;

                Console.WriteLine($"Configuring Base Path: {basePath}");

                var config = new ConfigurationBuilder()
                    .SetBasePath(basePath)
                    .AddJsonFile("Appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                var connectionString = config.GetConnectionString("SchoolDatabase");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Connection string 'SchoolDatabase' is missing or empty in Appsettings.json.");
                }

                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Teacher-Course (1:N)
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Teacher)
                .WithMany(t => t.Courses)
                .HasForeignKey(c => c.TeacherId);

            //Course-Assignment (1:N)
            modelBuilder.Entity<Assignement>()
                .HasOne(a => a.Course)
                .WithMany(c => c.Assignements)
                .HasForeignKey(a => a.CourseId);

            //Assignment-Submission (1:N)
            modelBuilder.Entity<Submission>()
                .HasOne(s => s.Assignement)
                .WithMany(a => a.Submissions)
                .HasForeignKey(s => s.AssignementId);

            //Student-Submission (1:N)
            modelBuilder.Entity<Submission>()
                .HasOne(s => s.Student)
                .WithMany(st => st.Submissions)
                .HasForeignKey(s => s.StudentId);

            //ValueConverter for Deadline property in Assignment
            modelBuilder.Entity<Assignement>()
                .Property(a => a.Deadline)
                .HasConversion(
                    d => d.ToString(),          //Pārvērš date objektu uz string datubāzei (1)
                    s => Date.Parse(s)          //Pārvērš string atpakaļ uz Date (1)
                );
        }
    }
}

/**
 * Atsauces:
 *     1. ChatGPT
 * **/