using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace BlogWebApi.Models;

public partial class BlogDbwebapiContext : DbContext
{
    public BlogDbwebapiContext()
    {
    }

    public BlogDbwebapiContext(DbContextOptions<BlogDbwebapiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=Blog_DBWEBAPI;Integrated Security=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__NewCateg__19093A2B35CC56BC");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("CategoryID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.PostId).HasName("PK__NewPost__AA126038826EBCD9");

            entity.ToTable("Post");

            entity.Property(e => e.PostId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("PostID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(255);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Category).WithMany(p => p.Posts)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Post_Category");

            entity.HasOne(d => d.User).WithMany(p => p.Posts)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_Post_User");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__NewRole__8AFACE3A3ACADFE7");

            entity.ToTable("Role");

            entity.Property(e => e.RoleId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("RoleID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__NewUser__1788CCAC70685AA9");

            entity.ToTable("User");

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("UserID");
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.Registered).HasColumnType("datetime");
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_User_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
