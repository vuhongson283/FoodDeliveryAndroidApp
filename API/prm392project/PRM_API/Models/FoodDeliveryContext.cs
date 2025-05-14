using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PRM_API.Models
{
    public partial class FoodDeliveryContext : DbContext
    {
        public FoodDeliveryContext()
        {
        }

        public FoodDeliveryContext(DbContextOptions<FoodDeliveryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; } = null!;
        public virtual DbSet<Food> Foods { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;
        public virtual DbSet<OrderDetail> OrderDetails { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("server =(local); database = FoodDelivery;uid=sa;pwd=1;TrustServerCertificate=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChatMessage>(entity =>
            {
                entity.HasKey(e => e.MessageId)
                    .HasName("PK__ChatMess__C87C037CABFFF1A1");

                entity.Property(e => e.MessageId).HasColumnName("MessageID");

                entity.Property(e => e.Sender).HasMaxLength(100);

                entity.Property(e => e.SentAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ChatMessages)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("FK__ChatMessa__UserI__4222D4EF");
            });

            modelBuilder.Entity<Food>(entity =>
            {
                entity.Property(e => e.FoodId).HasColumnName("FoodID");

                entity.Property(e => e.Image).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.CustomerName).HasMaxLength(500);

                entity.Property(e => e.OrderStatus).HasMaxLength(50);

                entity.Property(e => e.PaymentMethod).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsFixedLength();

                entity.Property(e => e.TotalPrice).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.UpdateAt).HasColumnType("datetime");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__Orders__UserID__44FF419A");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");

                entity.Property(e => e.FoodId).HasColumnName("FoodID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Food)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.FoodId)
                    .HasConstraintName("FK__OrderDeta__FoodI__4316F928");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .HasConstraintName("FK__OrderDeta__Order__440B1D61");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Users__A9D105347FADA6DA")
                    .IsUnique();

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Email).HasMaxLength(100);

                entity.Property(e => e.Password).HasMaxLength(255);

                entity.Property(e => e.PhoneNumber).HasMaxLength(20);

                entity.Property(e => e.Role).HasMaxLength(50);

                entity.Property(e => e.Username).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
