using System.ComponentModel.DataAnnotations.Schema;

namespace ApiSiad.Domain.Model
{
    public class AlunoCsvModel

    {
        [NotMapped]
        public int aluno_id { get; set; }
        public string nome { get; set; }
        public string turma_nome { get; set; }
        public decimal media_notas { get; set; }
        public string status { get; set; }
    }


}
