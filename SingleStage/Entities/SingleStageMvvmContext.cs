using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SingleStage.Entities;

public partial class SingleStageMvvmContext : DbContext
{
    public SingleStageMvvmContext()
    {
    }

    public SingleStageMvvmContext(DbContextOptions<SingleStageMvvmContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<ArtistPerformance> ArtistPerformances { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Performance> Performances { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<Show> Shows { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<Ticketholder> Ticketholders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=Single_Stage_MVVM;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Artist__3213E83FBF545CD8");

            entity.ToTable("Artist");

            entity.HasIndex(e => e.Name, "UQ__Artist__737584F6DE623A9F").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ArtistPerformance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ArtistPe__3213E83FAE029D8A");

            entity.ToTable("ArtistPerformance");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ArtistId).HasColumnName("Artist_id");
            entity.Property(e => e.PerformanceId).HasColumnName("Performance_id");
            entity.Property(e => e.RoyaltyAtEnd).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.RoyaltyUpFront).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Artist).WithMany(p => p.ArtistPerformances)
                .HasForeignKey(d => d.ArtistId)
                .HasConstraintName("FK_ParentArtistChildArtistPerformance");

            entity.HasOne(d => d.Performance).WithMany(p => p.ArtistPerformances)
                .HasForeignKey(d => d.PerformanceId)
                .HasConstraintName("FK_ParentPerformanceChildArtistPerformance");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83FC9EAA033");

            entity.ToTable("Employee");

            entity.HasIndex(e => e.Username, "UQ__Employee__536C85E4728EAFE2").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Performance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Performa__3213E83F43EC31A8");

            entity.ToTable("Performance");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.ShowId).HasColumnName("Show_id");
            entity.Property(e => e.StartTime).HasColumnType("datetime");

            entity.HasOne(d => d.Show).WithMany(p => p.Performances)
                .HasForeignKey(d => d.ShowId)
                .HasConstraintName("FK_ParentShowChildPerformance");
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Seat__3213E83FF5585E23");

            entity.ToTable("Seat");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Row)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<Show>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Show__3213E83F7FDAF752");

            entity.ToTable("Show");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.TicketPrice).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ticket__3213E83FC9DE0891");

            entity.ToTable("Ticket");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.SeatId).HasColumnName("Seat_id");
            entity.Property(e => e.ShowId).HasColumnName("Show_id");
            entity.Property(e => e.TicketholderId).HasColumnName("Ticketholder_id");

            entity.HasOne(d => d.Seat).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.SeatId)
                .HasConstraintName("FK_ParentSeatChildTicket");

            entity.HasOne(d => d.Show).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.ShowId)
                .HasConstraintName("FK_ParentShowChildTicket");

            entity.HasOne(d => d.Ticketholder).WithMany(p => p.Tickets)
                .HasForeignKey(d => d.TicketholderId)
                .HasConstraintName("FK_ParentTicketholderChildTicket");
        });

        modelBuilder.Entity<Ticketholder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ticketho__3213E83F9EA94329");

            entity.ToTable("Ticketholder");

            entity.HasIndex(e => e.Email, "UQ__Ticketho__A9D10534B3D2DF87").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Birthdate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
