﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OlivaTech.Site.Data;

namespace OlivaTech.Site.Migrations.ContextDbMigrations
{
    [DbContext(typeof(ContextDb))]
    [Migration("20210611205051_AddInitial")]
    partial class AddInitial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("OlivaTech.Site.Models.Curso", b =>
                {
                    b.Property<long>("CursoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Cidade")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("CursoTipoId")
                        .HasColumnType("bigint");

                    b.Property<bool>("Disponivel")
                        .HasColumnType("bit");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(42)
                        .HasColumnType("nvarchar(42)");

                    b.Property<string>("UF")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("nvarchar(2)");

                    b.HasKey("CursoId");

                    b.HasIndex("CursoTipoId");

                    b.ToTable("Cursos");
                });

            modelBuilder.Entity("OlivaTech.Site.Models.CursoTipo", b =>
                {
                    b.Property<long>("CursoTipoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("CursoTipoId");

                    b.ToTable("CursoTipos");
                });

            modelBuilder.Entity("OlivaTech.Site.Models.Curso", b =>
                {
                    b.HasOne("OlivaTech.Site.Models.CursoTipo", "CursoTipo")
                        .WithMany("Cursos")
                        .HasForeignKey("CursoTipoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CursoTipo");
                });

            modelBuilder.Entity("OlivaTech.Site.Models.CursoTipo", b =>
                {
                    b.Navigation("Cursos");
                });
#pragma warning restore 612, 618
        }
    }
}
