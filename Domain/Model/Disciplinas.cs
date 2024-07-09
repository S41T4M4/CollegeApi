using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiSiad.Domain.Model
{
    [Table("disciplinas")]
    public class Disciplinas
    {
        [Key]
        public int disciplina_id { get; set; }
        public string nome { get; set; }
    }
}
