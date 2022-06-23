using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Models
{
    public class Alunos 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Digite um nome válido.")]
        public string Nome { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Digite um CPF válido.")]
        public string Cpf { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Digite uma matrícula válida.")]
        public string Matricula { get; set; }
    }
}
