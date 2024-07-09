using ApiSiad.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ApiSiad.Domain;
using ApiSiad.Application.ViewModel;
using Microsoft.EntityFrameworkCore;


namespace ApiSiad.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/siad")]
    [ApiVersion("1.0")]
    public class SiadController : ControllerBase
    {
        private readonly ISiadRepository _siadRepository;

        public SiadController(ISiadRepository siadRepository)
        {
            _siadRepository = siadRepository;
        }

        // Métodos para turmas
        [HttpPost("turmas")]
        public IActionResult AddTurma(TurmasViewModel turmas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var turma = new Turmas
            {
                nome = turmas.Nome
            };

            _siadRepository.AddTurma(turma);

            return Ok();
        }

        [HttpPut("turmas/{turma_id}")]
        public IActionResult UpdateTurma(int turma_id, TurmasViewModel turmas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var turma = new Turmas
            {
                turma_id = turma_id,
                nome = turmas.Nome
            };

            _siadRepository.UpdateTurma(turma);

            return Ok();
        }

        [HttpGet("turmas")]
        public IActionResult GetTurmas()
        {
            var turmas = _siadRepository.GetTurmas();
            return Ok(turmas);
        }

        [HttpDelete("turmas/{turmaId}")]
        public IActionResult DeleteTurma(int turmaId)
        {
            try
            {
                _siadRepository.DeleteTurma(turmaId);
                return Ok();
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Erro ao excluir turma: {ex.Message}");
                return StatusCode(500, $"Erro ao excluir turma: {ex.Message}");
            }
        }

        // Métodos para alunos
        [HttpPost("alunos")]
        public IActionResult AddAluno([FromBody] AlunosViewModel alunoViewModel)
        {
            if (alunoViewModel == null || string.IsNullOrWhiteSpace(alunoViewModel.Nome) || alunoViewModel.Turma_Id <= 0)
            {
                return BadRequest("Dados do aluno inválidos.");
            }

            try
            {
                // Verificar se a turma existe
                var turma = _siadRepository.GetTurmaById(alunoViewModel.Turma_Id);
                if (turma == null)
                {
                    return BadRequest("A turma associada ao aluno não existe.");
                }

                var aluno = new Alunos
                {
                    nome = alunoViewModel.Nome,
                    turma_id = alunoViewModel.Turma_Id
                };

                _siadRepository.AddAluno(aluno);
                return Ok(aluno);
            }
            catch (Exception ex)
            {
                // Log detalhado da exceção
                Console.WriteLine($"Erro ao adicionar aluno: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return StatusCode(500, "Erro interno do servidor.");
            }
        }



        [HttpPut("alunos/{aluno_id}")]
        public IActionResult UpdateAluno(int aluno_id, AlunosViewModel aluno)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedAluno = new Alunos
            {
                aluno_id = aluno_id,
                nome = aluno.Nome,
                turma_id = aluno.Turma_Id
            };

            _siadRepository.UpdateAluno(updatedAluno);

            return Ok();
        }

        [HttpGet("alunos")]
        public IActionResult GetAlunos()
        {
            var alunos = _siadRepository.GetAlunos();
            return Ok(alunos);
        }

        [HttpDelete("alunos/{aluno_id}")]
        public IActionResult DeleteAluno(int aluno_id)
        {
            _siadRepository.DeleteAluno(aluno_id);
            return Ok();
        }


        [HttpPost("disciplinas")]
        public IActionResult AddDisciplina(DisciplinasViewModel disciplina)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var novaDisciplina = new Disciplinas
            {
                nome = disciplina.Nome
            };

            _siadRepository.AddDisciplina(novaDisciplina);

            return Ok();
        }

        [HttpPut("disciplinas/{disciplina_id}")]
        public IActionResult UpdateDisciplina(int disciplina_id, DisciplinasViewModel disciplina)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedDisciplina = new Disciplinas
            {
                disciplina_id = disciplina_id,
                nome = disciplina.Nome
            };

            _siadRepository.UpdateDisciplina(updatedDisciplina);

            return Ok();
        }

        [HttpGet("disciplinas")]
        public IActionResult GetDisciplinas()
        {
            var disciplinas = _siadRepository.GetDisciplinas();
            return Ok(disciplinas);
        }

        [HttpDelete("disciplinas/{disciplina_id}")]
        public IActionResult DeleteDisciplina(int disciplina_id)
        {
            _siadRepository.DeleteDisciplina(disciplina_id);
            return Ok();
        }


        [HttpPost("notas")]
        public IActionResult AddNota(NotasViewModel nota)
        {
            Console.WriteLine($"Aluno ID: {nota.Aluno_Id}, Disciplina ID: {nota.Disciplina_Id}, Nota: {nota.Nota}");

            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState inválido:");
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        Console.WriteLine($"{error.ErrorMessage}");
                    }
                }

                return BadRequest(ModelState);
            }

            var novaNota = new Notas
            {
                aluno_id = nota.Aluno_Id,
                disciplina_id = nota.Disciplina_Id,
                nota = nota.Nota
            };

            _siadRepository.AddNota(novaNota);

            return Ok();
        }


        [HttpPut("notas/{nota_id}")]
        public IActionResult UpdateNota(int nota_id, NotasViewModel nota)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedNota = new Notas
            {
                nota_id = nota_id,
                aluno_id = nota.Aluno_Id,
                disciplina_id = nota.Disciplina_Id,
                nota = nota.Nota
            };

            _siadRepository.UpdateNota(updatedNota);

            return Ok();
        }

        [HttpGet("notas")]
        public IActionResult GetNotas()
        {
            var notas = _siadRepository.GetNotas();
            return Ok(notas);
        }

        [HttpDelete("notas/{nota_id}")]
        public IActionResult DeleteNota(int nota_id)
        {
            _siadRepository.DeleteNota(nota_id);
            return Ok();
        }
        [HttpGet("turmas-alunos-disciplinas-notas")]
        public IActionResult GetTurmasAlunosDisciplinasNotas()
        {
            var result = _siadRepository.GetTurmasAlunosDisciplinasNotas();
            return Ok(result);
        }
    }
}