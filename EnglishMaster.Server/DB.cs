using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using EnglishMaster.Shared.Models;

namespace EnglishMaster.Server;

public partial class DB : DbContext
{
    public DB()
    {
    }

    public DB(DbContextOptions<DB> options)
        : base(options)
    {
    }

    public virtual DbSet<AccessToken> AccessTokens { get; set; }

    public virtual DbSet<Idiom> Idioms { get; set; }

    public virtual DbSet<Level> Levels { get; set; }

    public virtual DbSet<MeaningOfIdiom> MeaningOfIdioms { get; set; }

    public virtual DbSet<MeaningOfWord> MeaningOfWords { get; set; }

    public virtual DbSet<MeaningOfWordLearningHistory> MeaningOfWordLearningHistories { get; set; }

    public virtual DbSet<Mode> Modes { get; set; }

    public virtual DbSet<PartOfSpeech> PartOfSpeeches { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomUser> RoomUsers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Word> Words { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:DB");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccessToken>(entity =>
        {
            entity.HasKey(e => e.Token);

            entity.Property(e => e.Token)
                .HasMaxLength(64)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Expires).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.AccessTokens)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccessTokens_Users");
        });

        modelBuilder.Entity<Idiom>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Idiom1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Idiom");
        });

        modelBuilder.Entity<Level>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<MeaningOfIdiom>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.IdiomId).HasColumnName("IdiomID");
            entity.Property(e => e.Meaning).HasMaxLength(50);

            entity.HasOne(d => d.Idiom).WithMany(p => p.MeaningOfIdioms)
                .HasForeignKey(d => d.IdiomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MeaningOfIdioms_Idioms");
        });

        modelBuilder.Entity<MeaningOfWord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Vocabularies");

            entity.HasIndex(e => e.Id, "IX_Vocabularies");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.LevelId).HasColumnName("LevelID");
            entity.Property(e => e.Meaning).HasMaxLength(100);
            entity.Property(e => e.PartOfSpeechId).HasColumnName("PartOfSpeechID");
            entity.Property(e => e.WordId).HasColumnName("WordID");

            entity.HasOne(d => d.Level).WithMany(p => p.MeaningOfWords)
                .HasForeignKey(d => d.LevelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vocabularies_Levels");

            entity.HasOne(d => d.PartOfSpeech).WithMany(p => p.MeaningOfWords)
                .HasForeignKey(d => d.PartOfSpeechId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vocabularies_PartOfSpeech");

            entity.HasOne(d => d.Word).WithMany(p => p.MeaningOfWords)
                .HasForeignKey(d => d.WordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vocabularies_Words");
        });

        modelBuilder.Entity<MeaningOfWordLearningHistory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_MeaningOfWordHistories");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.ModeId).HasColumnName("ModeID");
            entity.Property(e => e.QuestionMeaningOfWordId).HasColumnName("QuestionMeaningOfWordID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Mode).WithMany(p => p.MeaningOfWordLearningHistories)
                .HasForeignKey(d => d.ModeId)
                .HasConstraintName("FK_MeaningOfWordLearningHistories_Modes");

            entity.HasOne(d => d.QuestionMeaningOfWord).WithMany(p => p.MeaningOfWordLearningHistories)
                .HasForeignKey(d => d.QuestionMeaningOfWordId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MeaningOfWordHistories_MeaningOfWords");

            entity.HasOne(d => d.User).WithMany(p => p.MeaningOfWordLearningHistories)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MeaningOfWordHistories_Users");
        });

        modelBuilder.Entity<Mode>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Description).HasColumnType("ntext");
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<PartOfSpeech>(entity =>
        {
            entity.ToTable("PartOfSpeech");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.InJapanese).HasMaxLength(50);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasIndex(e => e.Id, "IX_Rooms").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Code)
                .HasMaxLength(6)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(50);
        });

        modelBuilder.Entity<RoomUser>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.RoomId).HasColumnName("RoomID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Room).WithMany(p => p.RoomUsers)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoomUsers_Rooms");

            entity.HasOne(d => d.User).WithMany(p => p.RoomUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoomUsers_Users");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Username, "UQ_Users_Email").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.IconUrl)
                .HasMaxLength(1024)
                .IsUnicode(false);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.Nickname).HasMaxLength(15);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(254)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Word>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Word1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Word");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
