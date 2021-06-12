using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OlivaTech.Site.Models
{
    public class CursoTipo
    {
        [Key]
        public long CursoTipoId { get; set; }

        [Required(ErrorMessage = "O {0} é obrigatório.")]
        [Display(Name = "Formação")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "O {0} deve conter de {2} a {1} caracteres.")]
        public string Nome { get; set; }

        public List<Curso> Cursos { get; set; }
    }
}
