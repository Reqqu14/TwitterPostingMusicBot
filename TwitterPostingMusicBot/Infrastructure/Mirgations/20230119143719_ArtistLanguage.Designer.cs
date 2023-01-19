﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TwitterPostingMusicBot.Infrastructure;

#nullable disable

namespace TwitterPostingMusicBot.Migrations
{
    [DbContext(typeof(TwitterPostingMusicDbContext))]
    [Migration("20230119143719_ArtistLanguage")]
    partial class ArtistLanguage
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("TwitterPostingMusicBot.Models.Domain.Artist", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ArtistId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ArtistLanguage")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.Property<string>("ArtistName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ArtistTwitterName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("LastModifiedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTimeOffset?>("LastReleasedSongDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("ToUpdate")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Artists", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}
