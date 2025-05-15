using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace TermekekProjekt.Models;

public partial class TermekekContext : DbContext
{
    public TermekekContext()
    {
    }

    public TermekekContext(DbContextOptions<TermekekContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Purchase> Purchases { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlite("Data Source=Database\\termekek.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("category");

            entity.Property(e => e.Categoryname).HasColumnName("categoryname");
            entity.Property(e => e.Id)
                .HasColumnType("INT")
                .HasColumnName("id");

            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("customer");

            entity.Property(e => e.Budget)
                .HasColumnType("INT")
                .HasColumnName("budget");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.Name).HasColumnName("name");

            entity.HasKey(e => e.Name);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("product");

            entity.Property(e => e.Categoryid)
                .HasColumnType("INT")
                .HasColumnName("categoryid");
            entity.Property(e => e.Id)
                .HasColumnType("INT")
                .HasColumnName("id");
            entity.Property(e => e.Name).HasColumnName("name");
            entity.Property(e => e.Price)
                .HasColumnType("INT")
                .HasColumnName("price");
            entity.Property(e => e.Stock)
                .HasColumnType("INT")
                .HasColumnName("stock");

            entity.HasKey(e => e.Id);
        });

        modelBuilder.Entity<Purchase>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("purchase");

            entity.Property(e => e.CustomerId)
                .HasColumnType("INT")
                .HasColumnName("CustomerID");
            entity.Property(e => e.ProductId)
                .HasColumnType("INT")
                .HasColumnName("ProductID");
            entity.Property(e => e.Quantity).HasColumnType("INT");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
