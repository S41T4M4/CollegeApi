using ApiSiad.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;


namespace ApiSiad.Infraestrutura.Repositories
{
    public class SiadRepository : ISiadRepository
    {
        private readonly ConnectionContext _context = new ConnectionContext();
        public void AddTurma(Turmas turma)
        {
            _context.Turmas.Add(turma);
            _context.SaveChanges();
        }

        public void AddAluno(Alunos aluno)
        {
            var turmaExists = _context.Turmas.Any(t => t.turma_id == aluno.turma_id);
            if (!turmaExists)
            {
                throw new Exception("A turma associada ao aluno não existe.");
            }

            _context.Alunos.Add(aluno);
            _context.SaveChanges();
        }
        public Turmas GetTurmaById(int turmaId)
        {
            return _context.Turmas.FirstOrDefault(t => t.turma_id == turmaId);
        }
        public void AddDisciplina(Disciplinas disciplina)
        {
            if (disciplina == null)
            {
                throw new ArgumentNullException(nameof(disciplina));
            }

            
            if (string.IsNullOrEmpty(disciplina.nome))
            {
                throw new ArgumentException("O nome da disciplina é obrigatório.");
            }

            _context.Disciplinas.Add(disciplina);
            _context.SaveChanges();
        }

        public void AddNota(Notas nota)
        {

            var existingNota = _context.Notas
                .FirstOrDefault(n => n.aluno_id == nota.aluno_id && n.disciplina_id == nota.disciplina_id);

            if (existingNota != null)
            {
                throw new Exception($"Já existe uma nota cadastrada para o aluno {nota.aluno_id} na disciplina {nota.disciplina_id}");
            }

            _context.Notas.Add(nota);
            _context.SaveChanges();
        }



        public void DeleteAluno(int aluno_id)
        {
            var aluno = _context.Alunos.Find(aluno_id);
            if (aluno != null)
            {
                _context.Alunos.Remove(aluno);
                _context.SaveChanges();
            }
            else
            {
                throw new KeyNotFoundException($"Aluno with ID {aluno_id} not found.");
            }
        }

        public void DeleteDisciplina(int disciplina_id)
        {
            var disciplina = _context.Disciplinas.Find(disciplina_id);
            if (disciplina != null)
            {
                _context.Disciplinas.Remove(disciplina);
                _context.SaveChanges();
            }
        }

        public void DeleteNota(int nota_id)
        {
            var nota = _context.Notas.Find(nota_id);
            if (nota != null)
            {
                _context.Notas.Remove(nota);
                _context.SaveChanges();
            }
        }

        public void DeleteTurma(int turmaId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    // Obter todos os alunos da turma
                    var alunos = _context.Alunos.Where(a => a.turma_id == turmaId).ToList();

                    // Apagar as notas dos alunos
                    foreach (var aluno in alunos)
                    {
                        var notas = _context.Notas.Where(n => n.aluno_id == aluno.aluno_id).ToList();
                        _context.Notas.RemoveRange(notas);
                    }

                    // Apagar os alunos da turma
                    _context.Alunos.RemoveRange(alunos);

                    // Apagar a turma
                    var turma = _context.Turmas.Find(turmaId);
                    _context.Turmas.Remove(turma);

                    _context.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }


        public List<Turmas> GetTurmas()
        {
            return _context.Turmas.ToList();
        }

        public List<Alunos> GetAlunos()
        {
            return _context.Alunos.ToList();
        }

        public List<Disciplinas> GetDisciplinas()
        {
            return _context.Disciplinas.ToList();
        }

        public List<Notas> GetNotas()
        {
            return _context.Notas.ToList();
        }



        public void UpdateAluno(Alunos aluno)
        {
            var existingAluno = _context.Alunos.Find(aluno.aluno_id);
            if (existingAluno != null)
            {
                existingAluno.nome = aluno.nome;
                existingAluno.turma_id = aluno.turma_id;
                _context.Alunos.Update(existingAluno);
                _context.SaveChanges();
            }
        }

        public void UpdateDisciplina(Disciplinas disciplina)
        {
            _context.Disciplinas.Update(disciplina);
            _context.SaveChanges();
        }




        public void UpdateTurma(Turmas turma)
        {
            _context.Turmas.Update(turma);
            _context.SaveChanges();
        }

        public void UpdateNota(Notas nota)
        {
            _context.Notas.Update(nota);
            _context.SaveChanges();
        }

        public List<object> GetTurmasAlunosDisciplinasNotas()
        {
            var query = from turma in _context.Turmas
                        join aluno in _context.Alunos on turma.turma_id equals aluno.turma_id
                        join nota in _context.Notas on aluno.aluno_id equals nota.aluno_id
                        join disciplina in _context.Disciplinas on nota.disciplina_id equals disciplina.disciplina_id
                        select new
                        {
                            Turma = turma,
                            Aluno = aluno,
                            Disciplina = disciplina,
                            Nota = nota
                        };

            return query.ToList<object>();
        }

      
    }
}