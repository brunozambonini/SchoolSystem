using Application.Models;
using Application.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Refit;
using System;
using System.Threading.Tasks;

namespace Phidelis.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlunosController : ControllerBase
    {
        private readonly IAlunosService _studentService;

        public AlunosController(IAlunosService alunoService)
        {
            _studentService = alunoService;
        }

        // GET: api/Alunos
        /// <summary>
        /// Retorna todos lista de todos alunos, sem filtro
        /// </summary>
        /// <returns>Retorna lista de alunos</returns>
        /// <response code="200">Retorna lista de alunos</response>
        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var students = await _studentService.GetAll("");
                if (students != null)
                {
                    return Ok(students);
                }
                else
                {
                    return BadRequest("Página não encontrada.");
                }

            }
            catch (Exception ex)
            {

                return BadRequest($"Erro: {ex}");
            }
        }

        /// <summary>
        /// Retornar lista de aluno filtrando pelo nome
        /// </summary>
        /// <returns>Retorna lista de alunos</returns>
        /// <response code="200">Retorna lista de alunos</response>
        /// <param filter="termo usado para filtrar"></param>
        [HttpGet("{filter}")]
        public async Task<IActionResult> GetAllFiltering(string filter)
        {
            try
            {
                var students = await _studentService.GetAll(filter);
                if (students != null)
                {
                    return Ok(students);
                }
                else
                {
                    return BadRequest("Página não encontrada.");
                }

            }
            catch (Exception ex)
            {

                return BadRequest($"Erro: {ex}");
            }
        }

        //POST api/Aluno
        /// <summary>
        /// Cria um novo aluno
        /// </summary>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <param model="Objeto aluno (Id, Nome, CPF, Matricula)"></param>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Alunos model)
        {
            try
            {
                string erros = _studentService.AlunoValidate(model);
                if (!string.IsNullOrWhiteSpace(erros))
                {
                    // Retorna erro caso não seja válido
                    return BadRequest(erros);
                }

                // Salva no BD
                _studentService.Add(model);
                if (await _studentService.SaveChangeAsync())
                {
                    return Ok();
                }
                return BadRequest("Não foi possível salvar");
            }
            catch (Exception ex)
            {
                // Retorna erro com detalhes caso dê algum erro
                return BadRequest($"Erro: {ex}");
            }
        }


        //// PUT api
        /// <summary>
        /// Edita os dados do aluno ao passar o objeto do Aluno pelo Body
        /// </summary>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <param model="Objeto aluno (Id, Nome, CPF, Matricula)"></param>
        [HttpPut()]
        public async Task<IActionResult> Put([FromBody] Alunos model)
        {
            try
            {
                var student = await _studentService.GetAlunoById(model.Id);
                if (student != null)
                {
                    string errors = _studentService.AlunoValidate(model);
                    if (!string.IsNullOrWhiteSpace(errors))
                    {
                        // Retorna erro caso não seja válido
                        return BadRequest(errors);
                    }

                    _studentService.Update(model);
                    await _studentService.SaveChangeAsync();
                    return Ok();
                }
                else
                {
                    // Retorna erro caso não encontre o registro
                    return BadRequest("Não foi possível encontrar o aluno");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Erro: {ex}");
            }

        }

        // DELETE api/5
        /// <summary>
        /// Remove um aluno ao passar o ID
        /// </summary>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <param id="id do aluno"></param>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var student = await _studentService.GetAlunoById(id);
                if (student != null)
                {
                    _studentService.Delete(student);
                    await _studentService.SaveChangeAsync();
                    return Ok();
                }
                // Retorna erro caso não encontre o registro
                return BadRequest("Não foi possível encontrar o aluno");
            }
            catch (Exception ex)
            {

                return BadRequest($"Erro: {ex}");
            }
        }

        /// <summary>
        /// Remove todos os alunos matriculados
        /// </summary>
        /// <returns></returns>
        /// <response code="200"></response>
        [HttpDelete("all")]
        public async Task<IActionResult> ResetDataBase()
        {
            try
            {
                var students = await _studentService.GetAll("");

                _studentService.DeleteAll();
                await _studentService.SaveChangeAsync();
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest($"Erro: {ex}");
            }
        }

        //POST api/Aluno
        /// <summary>
        /// Gera aluno com nome, cpf e matrícula aletório. Informar quantidade de alunos a serem gerados.
        /// </summary>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <param quantity="quantidade de alunos a serem gerados"></param>
        [HttpPost("gerador/{quantity}")]
        public async Task<IActionResult> Post(int quantity)
        {
            try
            {
                if (quantity < 1)
                {
                    return BadRequest("Insira um valor maior do que 0.");
                }


                var studentsToCreate = await _studentService.GenerateAluno(quantity);
                _studentService.AddRange(studentsToCreate);

                if (await _studentService.SaveChangeAsync())
                {
                    return Ok();
                }

                return BadRequest("Não foi possível salvar");
            }
            catch (Exception ex)
            {
                // Retorna erro com detalhes caso dê algum erro
                return BadRequest($"Erro: {ex}");
            }
        }
    }
}
