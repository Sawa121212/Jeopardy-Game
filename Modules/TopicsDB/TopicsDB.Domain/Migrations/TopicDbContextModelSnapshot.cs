﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TopicDb.Domain;

namespace TopicDb.Domain.Migrations
{
    [DbContext(typeof(TopicDbContext))]
    partial class TopicDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.17");

            modelBuilder.Entity("TopicDb.Domain.Models.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ChatId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("CorrectAnswer")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("MessageId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Price")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("TopicId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("TopicId");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("TopicDb.Domain.Models.Topic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Topics");
                });

            modelBuilder.Entity("TopicDb.Domain.Models.Question", b =>
                {
                    b.HasOne("TopicDb.Domain.Models.Topic", "Topic")
                        .WithMany("Questions")
                        .HasForeignKey("TopicId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Topic");
                });

            modelBuilder.Entity("TopicDb.Domain.Models.Topic", b =>
                {
                    b.Navigation("Questions");
                });
#pragma warning restore 612, 618
        }
    }
}
