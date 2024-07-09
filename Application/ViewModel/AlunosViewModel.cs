using System.ComponentModel.DataAnnotations;

namespace ApiSiad.Application.ViewModel
{
    public class AlunosViewModel
    {
        public int Aluno_Id { get; set; }
        public string Nome { get; set; }
        public int Turma_Id { get; set; }
    }
}
