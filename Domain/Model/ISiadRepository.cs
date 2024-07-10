namespace ApiSiad.Domain.Model
{
    public interface ISiadRepository
    {
        void AddTurma(Turmas turma);
        List<Turmas> GetTurmas();
        void UpdateTurma(Turmas turma);
        void DeleteTurma(int turma_id);

        void AddAluno(Alunos aluno);
        List<Alunos> GetAlunos();
        void UpdateAluno(Alunos aluno);
        void DeleteAluno(int aluno_id);
        void AddDisciplina(Disciplinas disciplina);
        List<Disciplinas> GetDisciplinas();
        void UpdateDisciplina(Disciplinas disciplina);
        void DeleteDisciplina(int disciplina_id);

        void AddNota(Notas nota);
        List<Notas> GetNotas();
        void UpdateNota(Notas nota);
        void DeleteNota(int nota_id);
        List<object> GetTurmasAlunosDisciplinasNotas();

        Task<IEnumerable<Notas>> GetNotasByAlunoAsync(int aluno_id);
        Turmas GetTurmaById(int turmaId);
        decimal CalcularMediaNotas(IEnumerable<Notas> notas);
        List<AlunoCsvModel> ObterDadosParaCsv();

    }
}