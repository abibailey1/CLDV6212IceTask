using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApp.Models;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<AppointmentSlot> AppointmentSlots { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=aws-0-eu-west-2.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.ezwelwedbpodwodvwddd;Password=CloudDev@Emeris;SSL Mode=Require;Trust Server Certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasPostgresEnum("auth", "aal_level", new[] { "aal1", "aal2", "aal3" })
            .HasPostgresEnum("auth", "code_challenge_method", new[] { "s256", "plain" })
            .HasPostgresEnum("auth", "factor_status", new[] { "unverified", "verified" })
            .HasPostgresEnum("auth", "factor_type", new[] { "totp", "webauthn", "phone", "recovery_code" })
            .HasPostgresEnum("auth", "oauth_authorization_status", new[] { "pending", "approved", "denied", "expired" })
            .HasPostgresEnum("auth", "oauth_client_type", new[] { "public", "confidential" })
            .HasPostgresEnum("auth", "oauth_registration_type", new[] { "dynamic", "manual" })
            .HasPostgresEnum("auth", "oauth_response_type", new[] { "code" })
            .HasPostgresEnum("auth", "one_time_token_type", new[] { "confirmation_token", "reauthentication_token", "recovery_token", "email_change_token_new", "email_change_token_current", "phone_change_token" })
            .HasPostgresEnum("realtime", "action", new[] { "INSERT", "UPDATE", "DELETE", "TRUNCATE", "ERROR" })
            .HasPostgresEnum("realtime", "equality_op", new[] { "eq", "neq", "lt", "lte", "gt", "gte", "in", "like", "ilike", "is", "match", "imatch", "isdistinct" })
            .HasPostgresEnum("storage", "buckettype", new[] { "STANDARD", "ANALYTICS", "VECTOR" })
            .HasPostgresExtension("extensions", "pg_stat_statements")
            .HasPostgresExtension("extensions", "pgcrypto")
            .HasPostgresExtension("extensions", "uuid-ossp")
            .HasPostgresExtension("vault", "supabase_vault");

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("Appointments_pkey");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("now()");

            entity.HasOne(d => d.Slot).WithMany(p => p.Appointments).HasConstraintName("Appointments_SlotId_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Appointments).HasConstraintName("Appointments_UserId_fkey");
        });

        modelBuilder.Entity<AppointmentSlot>(entity =>
        {
            entity.HasKey(e => e.SlotId).HasName("AppointmentSlots_pkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Userid).HasName("Users_pkey");

            entity.Property(e => e.Userid).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Email).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.FirstName).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.LastName).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.PasswordHash).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.Role).HasDefaultValueSql("'Patient'::character varying");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
