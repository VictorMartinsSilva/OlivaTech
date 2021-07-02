using Microsoft.EntityFrameworkCore;
using OlivaTech.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OlivaTech.Site.Data
{
    public class ContextDb : DbContext
    {
        public ContextDb(DbContextOptions<ContextDb> options) :
            base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CursoTipo>().HasData(
                new CursoTipo { CursoTipoId = 1, Nome = "Pós-Graduação" },
                new CursoTipo { CursoTipoId = 2, Nome = "Bacharelado"},
                new CursoTipo { CursoTipoId = 3, Nome = "Técnico" },
                new CursoTipo { CursoTipoId = 4, Nome = "Mestrado"});

            modelBuilder.Entity<Curso>().HasData(
                new Curso { CursoTipoId = 2, CursoId = 1, Nome = "Sistema de Informação", Cidade = "Volta Redonda", UF = "RJ", Disponivel = true },
                new Curso { CursoTipoId = 1, CursoId = 2, Nome = "Tecnologias para Aplicações Web", Cidade = "Serrana", UF = "SP", Disponivel = true },
                new Curso { CursoTipoId = 3, CursoId = 3, Nome = "Informática", Cidade = "Volta Redonda", UF = "RJ", Disponivel = false });

            modelBuilder.Entity<Oferta>().HasData(
                
                new Oferta { CursoId = 1, OfertaId = 1, Preco = Convert.ToDecimal(306.76) , Disponivel = true },
                new Oferta { CursoId = 2, OfertaId = 2, Preco = Convert.ToDecimal(158.58) , Disponivel = true },
                new Oferta { CursoId = 3, OfertaId = 3, Preco = Convert.ToDecimal(230.50) , Disponivel = false });

            
        }

        public DbSet<Curso> Cursos { get; set; }
        public DbSet<CursoTipo> CursoTipos { get; set; }
        public DbSet<Oferta> Ofertas { get; set; }
       


    }
}
