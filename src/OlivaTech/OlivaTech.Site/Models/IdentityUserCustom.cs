using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OlivaTech.Site.Models
{
    public class IdentityUserCustom : IdentityUser
    {
        [Required(ErrorMessage = "O {0} é obrigatório.")]
        [Display(Name = "Nome")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O {0} deve conter de {2} a {1} caracteres.")]
        public string Nome { get; set; }

        [Display(Name = "Sobrenome")]
        public string Sobrenome { get; set; }

        [Required(ErrorMessage = "A {0} é obrigatória.")]
        [Display(Name = "Cidade")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "A {0} deve conter de {2} a {1} caracteres.")]
        public string Cidade { get; set; }

        [Required(ErrorMessage = "O {0} é obrigatório.")]
        [Display(Name = "Bairro")]
        [StringLength(120, MinimumLength = 2, ErrorMessage = "O {0} deve conter de {2} a {1} caracteres.")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "O {0} é obrigatório.")]
        [Display(Name = "UF")]
        [StringLength(2, MinimumLength = 2, ErrorMessage = "O {0} deve conter de {2} a {1} caracteres.")]
        public string UF { get; set; }

        [Required(ErrorMessage = "O {0} é obrigatório.")]
        [Display(Name = "CEP")]
        [StringLength(9, MinimumLength = 8, ErrorMessage = "O {0} deve conter de {2} a {1} caracteres.")]
        public string CEP { get; set; }
    }
}
