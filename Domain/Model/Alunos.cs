using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiSiad.Domain.Model
{
    [Table("alunos")]
    public class Alunos
    {
        [Key]
        public int aluno_id { get; set; }

        [Required]
        [MaxLength(200)]
        public string nome { get; set; }

        [ForeignKey("Turmas")] 
        public int turma_id { get; set; }

        public Turmas Turma { get; set; } 
    }
}