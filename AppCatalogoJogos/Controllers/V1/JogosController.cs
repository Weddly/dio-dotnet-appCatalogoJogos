using AppCatalogoJogos.Exceptions;
using AppCatalogoJogos.InputModel;
using AppCatalogoJogos.Services;
using AppCatalogoJogos.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppCatalogoJogos.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class JogosController : ControllerBase
    {
        private readonly IJogoService _jogoService;

        public JogosController(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }
        /// <summary>
        /// Buscar todos os jogos de forma paginada
        /// </summary>
        /// <remarks>
        /// Não é possível retornar os jogos sem paginação
        /// </remarks>
        /// <param name="pagina">Indica qual a página que está sendo consultado. Mínimo 1</param>
        /// <param name="quantidade">Indica a quantidade de registros por página. Mínimo 1 e máximo 50</param>
        /// <response code="200">Retorna a lista de jogos</response>
        /// <response code="204">Caso não haja jogos</response>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JogoViewModel>>> Obter([FromQuery, Range(1, int.MaxValue)] int pagina = 1, [FromQuery, Range(1, 50)] int quantidade = 5)
        {
            var jogos = await _jogoService.Obter(pagina, quantidade);

            if (jogos.Count() == 0)
                return NoContent();
            return Ok(jogos);
        }

        /// <summary>
        /// Buscar o jogo pelo seu Id
        /// </summary>
        /// <param name="idJogo">Id do jogo buscado</param>
        /// <response code="200">Retorna o jogo buscado</response>
        /// <response code="204">Caso não haja jogo com esse Id</response>
        /// <returns></returns>
        [HttpGet("{idJogo:guid}")]
        public async Task<ActionResult<JogoViewModel>> Obter([FromRoute]Guid idJogo)
        {
            var jogo = await _jogoService.Obter(idJogo);

            if (jogo == null)
                return NoContent();
            return Ok(jogo);
        }

        /// <summary>
        /// Inserir jogo no banco de dados
        /// </summary>
        /// <param name="jogoInputModel">Objeto jogo a ser adicionado</param>
        /// <response code="200">Retorna o jogo buscado</response>
        /// <response code="422">Caso já exista um jogo com este nome e produtora cadastrado</response>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<JogoViewModel>> InserirJogo([FromBody] JogoInputModel jogoInputModel)
        {
            try
            {
                var jogo = await _jogoService.Inserir(jogoInputModel);
                return Ok(jogo);
            }
            catch (JogoJaCadastradoException ex)
            {
                return UnprocessableEntity("Já existe um jogo com este nome para esta produtora.");
            }
        }

        /// <summary>
        /// Alterar jogo existente no banco de dados
        /// </summary>
        /// <param name="idJogo">Id do jogo a ser alterado</param>
        /// <param name="jogoInputModel">Objeto jogo a ser alterado</param>
        /// <response code="200">Retorna sucesso</response>
        /// <response code="404">Caso não exista jogo cadastrado com o Id informado</response>
        [HttpPut("{idJogo:guid}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute]Guid idJogo, [FromBody] JogoInputModel jogoInputModel)
        {
            try
            {
                await _jogoService.Atualizar(idJogo, jogoInputModel);
                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound("Não existe este jogo.");
            }
        }

        /// <summary>
        /// Alterar preço de jogo existente no banco de dados
        /// </summary>
        /// <param name="idJogo">Id do jogo a ser alterado</param>
        /// <param name="preco">Valor a ser aplicado no jogo</param>
        /// <response code="200">Retorna sucesso</response>
        /// <response code="404">Caso não exista jogo cadastrado com o Id informado</response>
        [HttpPatch("{idJogo:guid}/preco/{preco:double}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute]Guid idJogo, [FromRoute] double preco)
        {
            try
            {
                await _jogoService.Atualizar(idJogo, preco);
                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound("Não existe este jogo.");
            }
        }

        /// <summary>
        /// Apaga jogo existente no banco de dados
        /// </summary>
        /// <param name="idJogo">Id do jogo a ser deletado</param>
        /// <response code="200">Retorna sucesso</response>
        /// <response code="404">Caso não exista jogo cadastrado com o Id informado</response>
        /// <returns></returns>
        [HttpDelete("{idJogo:guid}")]
        public async Task<ActionResult> ApagarJogo([FromRoute] Guid idJogo)
        {
            try
            {
                await _jogoService.Remover(idJogo);
                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound("Não existe este jogo.");
            }
        }
    }
}
