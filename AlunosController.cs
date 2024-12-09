using AlunosApi.Models;
using AlunosApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlunosApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Produces("application/json")]
    public class AlunosController : ControllerBase
    {
        private readonly IAlunoService _alunoService;

        public AlunosController(IAlunoService service)
        {
            _alunoService = service;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IAsyncEnumerable<Aluno>>> GetAlunos()
        {
            try
            {
                var alunos = await _alunoService.GetAlunos();
                return Ok(alunos);
            }
            catch
            {
                //return BadRequest("Request inválido");
                return StatusCode(StatusCodes.Status500InternalServerError, 
                    "Erro ao obter alunos");
            }
            
        }
        
        [HttpGet("AlunosPorNome")]
        public async Task<ActionResult<IAsyncEnumerable<Aluno>>> GetAlunosByNome([FromQuery] string nome)
        {
            try
            {
                var alunos = await _alunoService.GetAlunosByNome(nome);
                if (alunos == null)
                {
                    return NotFound($"Não existem alunos com o critério {nome}");
                }
                
                return Ok(alunos);
            }
            catch
            {
                return BadRequest("Request inválido");                
            }
        }

        [HttpGet("{id:int}", Name="GetAluno")]
        public async Task<ActionResult<IAsyncEnumerable<Aluno>>> GetAluno(int id)
        {
            try
            {
                var aluno = await _alunoService.GetAluno(id);
                if (aluno == null)
                {
                    return NotFound($"Não existe aluno com o id igual a {id}");                    
                }

                return Ok(aluno);
            }
            catch
            {
                return BadRequest("Request inválido");
            }
        }

        [HttpPost]
        public async Task<ActionResult> Create(Aluno aluno)
        {
            try
            {                
                await _alunoService.CreateAluno(aluno);

                return CreatedAtRoute(nameof(GetAluno), new { id = aluno.Id }, aluno); // retorna a uri do recuros criado no campo location
                //return Ok(aluno); não retorna a uri do recurso criado
            }
            catch
            {
                return BadRequest("Request inválido");
            }
        }


        [HttpPut("{id=int}")]        
        public async Task<ActionResult> Edit(int id, [FromBody] Aluno aluno)
        {
            try
            {
                if (aluno.Id == id)
                {
                    await _alunoService.UpdateAluno(aluno);
                    //return NoContent(); //retorna status http 204                    
                    return Ok($"Aluno com id={id} foi atualizado com sucesso");
                }
                else
                {
                    return BadRequest("Dados inconsistentes");
                }
                                
            }
            catch
            {
                return BadRequest("Request inválido");
            }
        }

        [HttpDelete("{id=int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var aluno = await _alunoService.GetAluno(id);
                if (aluno != null)
                {
                    await _alunoService.DeleteAluno(aluno);                   
                    return Ok($"Aluno com id={id} foi removido com sucesso");
                }
                else
                {
                    return NotFound($"Aluno com id={id} não foi encontrado");
                }

            }
            catch
            {
                return BadRequest("Request inválido");
            }
        }

    }
}
