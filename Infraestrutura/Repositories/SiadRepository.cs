using ApiSiad.Domain.Model;
using Microsoft.AspNetCore.Http.HttpResults;
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

            _context.Notas.Add(nota);
            _context.SaveChanges();
        }



        public void DeleteAluno(int aluno_id)
        {
            try
            {
                var aluno = _context.Alunos.Find(aluno_id);
                if (aluno != null)
                {
                    _context.Alunos.Remove(aluno);
                    _context.SaveChanges();
                }
            
            }
            catch (Exception ex)
            {
                Console.WriteLine("Impossivel deletar um aluno com notas cadastradas");
              
            }
        }

        public void DeleteDisciplina(int disciplina_id)
        {
            try
            {
                var disciplina = _context.Disciplinas.Find(disciplina_id);
                if (disciplina != null)
                {
                    _context.Disciplinas.Remove(disciplina);
                    _context.SaveChanges();
                }
                else
                {
                    throw new Exception("Impossivel deletar uma disciplina com alunos com notas cadastradas");
                }
            }
            catch
            {
                Console.WriteLine("Impossivel deletar disciplina com alunos");
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
            else
            {
                throw new Exception("Não foi possivel deletar a nota");
            }
        }

        public void DeleteTurma(int turmaId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                   
                    var alunos = _context.Alunos.Where(a => a.turma_id == turmaId).ToList();

          
                    foreach (var aluno in alunos)
                    {
                        var notas = _context.Notas.Where(n => n.aluno_id == aluno.aluno_id).ToList();
                        _context.Notas.RemoveRange(notas);
                    }

                    _context.Alunos.RemoveRange(alunos);

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
        public async Task<IEnumerable<Notas>> GetNotasByAlunoAsync(int aluno_id)
        {
            return await _context.Notas
                .Where(n => n.aluno_id == aluno_id)
                .ToListAsync();
        }

        public decimal CalcularMediaNotas(IEnumerable<Notas> notas)
        {
            if (notas == null || !notas.Any())
                return 0;

            decimal somaNotas = notas.Sum(n => n.nota);
            return somaNotas / notas.Count();
        }
        public List<AlunoCsvModel> ObterDadosParaCsv()
        {
            List<AlunoCsvModel> alunosCsv = new List<AlunoCsvModel>();

            var alunos = _context.Alunos.ToList();

            foreach (var aluno in alunos)
            {
                var turma = _context.Turmas.FirstOrDefault(t => t.turma_id == aluno.turma_id);
                var notas = _context.Notas.Where(n => n.aluno_id == aluno.aluno_id).ToList();
                decimal mediaNotas = CalcularMediaNotas(notas);
                string status = mediaNotas >= 6 ? "Aprovado" : "Reprovado";

                alunosCsv.Add(new AlunoCsvModel
                {
                    aluno_id = aluno.aluno_id,
                    nome = aluno.nome,
                    turma_nome = turma != null ? turma.nome : "N/A",
                    media_notas = mediaNotas,
                    status = status
                });
            }

            return alunosCsv;
        }

    }
}