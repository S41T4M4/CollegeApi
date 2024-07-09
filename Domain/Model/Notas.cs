using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiSiad.Domain.Model
{
    [Table("notas")]
    public class Notas
    {
        [Key]
        public int nota_id { get; set; }

        [Required]
        public decimal nota { get; set; }

        [ForeignKey("Alunos")]
        public int aluno_id { get; set; }

        [ForeignKey("Disciplinas")]
        public int disciplina_id { get; set; }
    }
}
