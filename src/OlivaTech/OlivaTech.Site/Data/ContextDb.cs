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

        public DbSet<Curso> Cursos { get; set; }
        public DbSet<CursoTipo> CursoTipos { get; set; }
       


    }
}
