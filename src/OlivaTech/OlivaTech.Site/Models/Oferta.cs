using OlivaTech.Site.Extension;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace OlivaTech.Site.Models
{
    public class Oferta
    {
        [Key]
        public long OfertaId { get; set; }

        [Required(ErrorMessage = "O {0} é obrigatório.")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(12,2)")]
        [Moeda]
        public decimal Preco { get; set; }

        [Required(ErrorMessage ="É obrigatório informar se a oferta está {0}") ]
        [Display(Name = "Disponível")]
        public bool Disponivel { get; set; }

        public Curso Curso { get; set; }
        public long CursoId { get; set; }
    }
}
