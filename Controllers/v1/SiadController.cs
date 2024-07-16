using ApiSiad.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ApiSiad.Domain;
using ApiSiad.Application.ViewModel;
using Microsoft.EntityFrameworkCore;
using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using System.Text;


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

      
        [HttpPost("turmas")]
        public IActionResult AddTurma(TurmasViewModel turmas)
        {
          
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
                
                return BadRequest(new { message = ex.Message });
            }
        }


        [HttpPost("alunos")]
        public IActionResult AddAluno([FromBody] AlunosViewModel alunoViewModel)
        {
            if (string.IsNullOrWhiteSpace(alunoViewModel.Nome))
            {
                return BadRequest("Dados do aluno inválidos.");
            }
            if (alunoViewModel.Nome.Length > 20)
            {
                return BadRequest("O nome do aluno ultrapassas o limite de caracteres ");
            }
            try
            {
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
            catch (Exception)
            {
                return StatusCode(500);
            }
        }



        [HttpPut("alunos/{aluno_id}")]
        public IActionResult UpdateAluno(int aluno_id, AlunosViewModel aluno)
        {
            try
            {
                var editarAluno = new Alunos
                {
                    aluno_id = aluno_id,
                    nome = aluno.Nome,
                    turma_id = aluno.Turma_Id
                };

                _siadRepository.UpdateAluno(editarAluno);

                return Ok();
            }
            catch (Exception)
            {
                return BadRequest("Erro ao editar o Aluno ");

            }
        }


        [HttpGet("alunos")]
        public IActionResult GetAlunos()
        {
            try
            {
                var alunos = _siadRepository.GetAlunos();
                return Ok(alunos);
            }
            catch (Exception)
            {
                return NotFound();

            }

        }


        [HttpDelete("alunos/{aluno_id}")]
        public IActionResult DeleteAluno(int aluno_id)
        {
            try
            {
                _siadRepository.DeleteAluno(aluno_id);
                return Ok();
            }
            catch
            {
                throw new Exception("Erro ao tentar deletar Aluno");
            }
        }



        [HttpPost("disciplinas")]
        public IActionResult AddDisciplina(DisciplinasViewModel disciplina)
        {
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

            var editarDisciplina = new Disciplinas
            {
                disciplina_id = disciplina_id,
                nome = disciplina.Nome
            };

            _siadRepository.UpdateDisciplina(editarDisciplina);

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
            try
            {
                _siadRepository.DeleteDisciplina(disciplina_id);
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(new { message = ex.Message });
            }

        }


        [HttpPost("notas")]
        public IActionResult AddNota(NotasViewModel nota)
        {
            Console.WriteLine($"Aluno ID : {nota.Aluno_Id}, Disciplina ID: {nota.Disciplina_Id}, Nota: {nota.Nota}");

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
        [HttpGet("alunos/{aluno_id}/notas")]
        public async Task<ActionResult<IEnumerable<Notas>>> GetNotasByAluno(int aluno_id)
        {
            var notas = await _siadRepository.GetNotasByAlunoAsync(aluno_id);

            if (notas == null)
            {
                return NotFound(new { message = "Nenhuma nota encontrada para este aluno." });
            }

            return Ok(notas);
        }


        [HttpGet("alunos/{aluno_id}/media")]
        public async Task<ActionResult<decimal>> GetMediaNotasByAluno(int aluno_id)
        {
            var notas = await _siadRepository.GetNotasByAlunoAsync(aluno_id);

            if (notas == null)
            {
                return NotFound(new { message = "Nenhuma nota encontrada para este aluno." });
            }

            var media = _siadRepository.CalcularMediaNotas(notas);
            return Ok(media);
        }
        [HttpGet("download-alunos-notas-csv")]
        public IActionResult DownloadAlunosNotasCSV()
        {
            try
            {
                var alunosNotasList = _siadRepository.ObterDadosParaCsv();
                var memoryStream = new MemoryStream();

                using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8, 1024, true))
                {
                    using (var csvWriter = new CsvWriter(streamWriter, new CsvConfiguration(CultureInfo.InvariantCulture)))
                    {
                        csvWriter.WriteRecords(alunosNotasList);
                    }
                }

                memoryStream.Seek(0, SeekOrigin.Begin);

                return File(memoryStream, "text/csv", "Download_Alunos_Notas.csv");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao gerar CSV: {ex.Message}");
            }
        }

    }
}