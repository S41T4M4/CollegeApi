using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiSiad.Domain.Model
{
    [Table("turmas")]
    public class Turmas
    {
        [Key]
        public int turma_id { get; set; }

        [Required]
        [MaxLength(255)]
        public string nome { get; set; }
    
 
    }
}
