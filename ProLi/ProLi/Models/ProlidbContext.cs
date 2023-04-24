using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ProLi.Models;

public partial class ProlidbContext : DbContext
{
    public ProlidbContext()
    {
    }

    public ProlidbContext(DbContextOptions<ProlidbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Prolievent> Prolievents { get; set; }

    public virtual DbSet<Proliimage> Proliimages { get; set; }

    public virtual DbSet<Proliinvite> Proliinvites { get; set; }

    public virtual DbSet<Prolioffice> Prolioffices { get; set; }

    public virtual DbSet<Proliperson> Prolipeople { get; set; }

  //  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseMySql("server=127.0.0.1;port=3306;database=prolidb;uid=root;password=Random", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.32-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Prolievent>(entity =>
        {
            entity.HasKey(e => e.EventId).HasName("PRIMARY");

            entity.ToTable("prolievent", tb => tb.HasComment("A rendezvények legfontosabb paramétereit tartalmazza a tábla."));

            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.EventDate)
                .HasComment("A rendezvény dátuma.")
                .HasColumnType("datetime");
            entity.Property(e => e.EventHead).HasComment("A rendezvényre meghívni tervezett személyek létszáma. A maximális létszámot ajánlott megadni.");
            entity.Property(e => e.EventName)
                .HasMaxLength(45)
                .HasComment("A rendezvény megnevezése.");
            entity.Property(e => e.EventPlace)
                .HasMaxLength(100)
                .HasComment("A rendezvény helyszínének megadása");
        });

        modelBuilder.Entity<Proliimage>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("PRIMARY");

            entity.ToTable("proliimage");

            entity.Property(e => e.PersonId).HasColumnName("PersonID");
            entity.Property(e => e.ImagePathl).HasMaxLength(500);
        });

        modelBuilder.Entity<Proliinvite>(entity =>
        {
            entity.HasKey(e => new { e.InviteId, e.EventId, e.PersonId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("proliinvite");

            entity.HasIndex(e => e.EventId, "fk_Invite_Event1_idx");

            entity.HasIndex(e => e.PersonId, "fk_Invite_Person1_idx");

            entity.Property(e => e.InviteId)
                .ValueGeneratedOnAdd()
                .HasColumnName("InviteID");
            entity.Property(e => e.EventId).HasColumnName("EventID");
            entity.Property(e => e.PersonId).HasColumnName("PersonID");

            entity.HasOne(d => d.Event).WithMany(p => p.Proliinvites)
                .HasForeignKey(d => d.EventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Invite_Event1");

            entity.HasOne(d => d.Person).WithMany(p => p.Proliinvites)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_Invite_Person1");
        });

        modelBuilder.Entity<Prolioffice>(entity =>
        {
            entity.HasKey(e => e.OfficeId).HasName("PRIMARY");

            entity.ToTable("prolioffice", tb => tb.HasComment("A megadott személy által betöltött jogviszony adatait tartalmazza a tábla. \nA személy egyedi azonosítója alapján rendeljük a személyhez a szervezetetet, amelyet  legalább egy legfeljebb három szinten azonosítunk. Tartalmazza a betöltött tisztséget. A jogviszony kezdetét (kötelezően) és végét (opcionális). \nAmennyiben a jogviszony vége nincs megadva, akkor az adott szervezetnél a megadott pozícióban aktív státuszban áll.\nEgy időben egy személy nulla, egy vagy több pozíciót is betölthet."));

            entity.HasIndex(e => e.PersonId, "fk_Office_Person1_idx");

            entity.Property(e => e.OfficeId).HasColumnName("OfficeID");
            entity.Property(e => e.OfficeAddress).HasMaxLength(100);
            entity.Property(e => e.OfficeEmail).HasMaxLength(50);
            entity.Property(e => e.OfficeEnd)
                .HasComment("Az adott szervezetnél, az adott jogviszony befejezésének dátuma. Üres érték esetén a jogviszony aktívnak tekintendő.")
                .HasColumnType("datetime");
            entity.Property(e => e.OfficeName1)
                .HasMaxLength(50)
                .HasComment("A szervezet 1. szintjének megnevezése. Kötelező megadni legalább egy szinten a szervezeti struktúrát, ahol az adott személy tevékenykedik a megadott pozícióban.");
            entity.Property(e => e.OfficeName2).HasMaxLength(50);
            entity.Property(e => e.OfficeName3).HasMaxLength(50);
            entity.Property(e => e.OfficePhone).HasMaxLength(20);
            entity.Property(e => e.OfficePost)
                .HasMaxLength(45)
                .HasComment("A betöltött pozíció, munkakör megnevezése. ");
            entity.Property(e => e.OfficeStart)
                .HasComment("Az adott szervezetnél, az adott jogviszony kezdetének dátuma.")
                .HasColumnType("datetime");
            entity.Property(e => e.PersonId).HasColumnName("PersonID");
        });

        modelBuilder.Entity<Proliperson>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("PRIMARY");

            entity.ToTable("proliperson", tb => tb.HasComment("Személyek nyilvántartására szolgáló tábla."));

            entity.Property(e => e.PersonId).HasColumnName("PersonID");
            entity.Property(e => e.PersonComment)
                .HasMaxLength(500)
                .HasComment("Szöveges megjegyzés hozzáadása.");
            entity.Property(e => e.PersonCountry)
                .HasMaxLength(30)
                .HasComment("Ország");
            entity.Property(e => e.PersonEmail)
                .HasMaxLength(100)
                .HasComment("1. e-mail cím");
            entity.Property(e => e.PersonName)
                .HasMaxLength(100)
                .HasComment("A nyilvántartott személy teljes neve.");
            entity.Property(e => e.PersonPhone)
                .HasMaxLength(15)
                .HasComment("1. Telefonszám");
            entity.Property(e => e.PersonSpecComment).HasMaxLength(500);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
