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

    public virtual DbSet<Appearance> Appearances { get; set; }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Seat> Seats { get; set; }

    public virtual DbSet<Show> Shows { get; set; }

    public virtual DbSet<ShowAppearance> ShowAppearances { get; set; }

    public virtual DbSet<Ticket> Tickets { get; set; }

    public virtual DbSet<Ticketholder> Ticketholders { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=Single_Stage_MVVM;Integrated Security=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appearance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Appearan__3213E83F5C2F695B");

            entity.ToTable("Appearance");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.ArtistId).HasColumnName("Artist_id");
            entity.Property(e => e.RoyaltyAtEnd).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.RoyaltyUpFront).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ShowAppearanceId).HasColumnName("ShowAppearance_id");

            entity.HasOne(d => d.Artist).WithMany(p => p.Appearances)
                .HasForeignKey(d => d.ArtistId)
                .HasConstraintName("FK_ParentArtistChildAppearance");

            entity.HasOne(d => d.ShowAppearance).WithMany(p => p.Appearances)
                .HasForeignKey(d => d.ShowAppearanceId)
                .HasConstraintName("FK_ParentShowAppearanceChildAppearance");
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Artist__3213E83FF432A587");

            entity.ToTable("Artist");

            entity.HasIndex(e => e.Name, "UQ__Artist__737584F65A06BDF0").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3213E83FDD45F866");

            entity.ToTable("Employee");

            entity.HasIndex(e => e.Username, "UQ__Employee__536C85E4610A01CC").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Seat__3213E83FDED4506F");

            entity.ToTable("Seat");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Row)
                .HasMaxLength(1)
                .IsUnicode(false)
                .IsFixedLength();
        });

        modelBuilder.Entity<Show>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Show__3213E83F6D2C66D4");

            entity.ToTable("Show");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.StartTime).HasColumnType("datetime");
            entity.Property(e => e.TicketPrice).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<ShowAppearance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ShowAppe__3213E83FA8157292");

            entity.ToTable("ShowAppearance");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Description)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.ShowId).HasColumnName("Show_id");
            entity.Property(e => e.StartTime).HasColumnType("datetime");

            entity.HasOne(d => d.Show).WithMany(p => p.ShowAppearances)
                .HasForeignKey(d => d.ShowId)
                .HasConstraintName("FK_ParentShowChildShowAppearance");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Ticket__3213E83F69D244E2");

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
            entity.HasKey(e => e.Id).HasName("PK__Ticketho__3213E83F5C46502C");

            entity.ToTable("Ticketholder");

            entity.HasIndex(e => e.Email, "UQ__Ticketho__A9D105349B658946").IsUnique();

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
