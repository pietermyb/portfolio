using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MySQL.Data.EntityFrameworkCore.Extensions;

using Portfolio.Model.Entities;

namespace Portfolio.Data.Context
{
    public class PortfolioContext : DbContext
    {
        public DbSet<Project> Projects { get; set; }
        public DbSet<Employment> EmploymentHistory { get; set; }
        public DbSet<SkillGroup> SkillGroups { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<History> HistoryInfo { get; set; }

        
        public PortfolioContext(DbContextOptions<PortfolioContext> options) : base(options)
        {
        }

        //Context factory to create database if it doesn't exist
        public static class PortfolioContextFactory
        {
            public static PortfolioContext Create(string connectionString)
            {
                var optionsBuilder = new DbContextOptionsBuilder<PortfolioContext>();
                optionsBuilder.UseMySQL(connectionString);

                //Ensure database creation
                var context = new PortfolioContext(optionsBuilder.Options);
                context.Database.EnsureCreated();

                return context;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            //Employment
            modelBuilder.Entity<Employment>()
                .ToTable("Employment");

            modelBuilder.Entity<Employment>()
                .Property(p => p.Id)
                .IsRequired();

            modelBuilder.Entity<Employment>()
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();
            
            modelBuilder.Entity<Employment>()
                .Property(p => p.Description)
                .HasMaxLength(500)
                .IsRequired();

            //Project
            modelBuilder.Entity<Project>()
                .ToTable("Project");

            modelBuilder.Entity<Project>()
                .Property(p => p.Id)
                .IsRequired();
           
            modelBuilder.Entity<Project>()
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Project>()
                .Property(p => p.Title)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Project>()
                .Property(p => p.Description)
                .HasMaxLength(500)
                .IsRequired();

            //History
            modelBuilder.Entity<History>()
                .ToTable("History");

            modelBuilder.Entity<History>()
                .Property(p => p.Id)
                .IsRequired();

            modelBuilder.Entity<History>()
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<History>()
                .Property(p => p.Description)
                .HasMaxLength(500)
                .IsRequired();
            
            //Skill
            modelBuilder.Entity<Skill>()
                .ToTable("Skill");

            modelBuilder.Entity<Skill>()
                .Property(p => p.Id)
                .IsRequired();

            modelBuilder.Entity<Skill>()
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<Skill>()
                .Property(p => p.Description)
                .HasMaxLength(500)
                .IsRequired();
            
            modelBuilder.Entity<Skill>()
                .HasOne(s => s.SkillGroup)
                .WithMany(sg => sg.Skills)
                .HasForeignKey(a => a.SkillGroupId);

            //Skill Group
            modelBuilder.Entity<SkillGroup>()
                .ToTable("SkillGroup");

            modelBuilder.Entity<SkillGroup>()
                .Property(p => p.Id)
                .IsRequired();

            modelBuilder.Entity<SkillGroup>()
                .Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            modelBuilder.Entity<SkillGroup>()
                .Property(p => p.Description)
                .HasMaxLength(500)
                .IsRequired();

            
        }
    }
}