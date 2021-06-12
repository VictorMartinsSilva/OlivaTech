using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OlivaTech.Site.Models
{
    public class Curso
    {
        [Key]
        public long CursoId { get; set; }

        [Required(ErrorMessage = "O {0} é obrigatório.")]
        [Display(Name = "Nome")]
        [StringLength(42, MinimumLength = 2, ErrorMessage = "O {0} é obrigatório conter de {2} a {1} caracteres.")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A {0} é obrigatória.")]
        [Display(Name = "Cidade")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "O {0} é obrigatório.")]
        [Display(Name = "UF")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "O {0} é obrigatório conter de {2} a {1} caracteres.")]
        public string UF { get; set; }

        [Required(ErrorMessage = "A {0} é obrigatória.")]
        [Display(Name = "Disponibilidade")]
        public Boolean Disponivel { get; set; }

        
        public CursoTipo CursoTipo { get; set; }
        public long CursoTipoId { get; set; }

    }
}
