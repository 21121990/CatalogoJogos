using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.InputModel;
using WebApplication1.Services;
using WebApplication1.ViewModel;
using System.ComponentModel.DataAnnotations;


namespace WebApplication1.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class Jogos : ControllerBase
    {
        private readonly IjogoService _jogoService;

        public Jogos(IjogoService jogoService)
        {
            _jogoService = jogoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JogoViewModel>>> Obter([FromQuery, Range(1, int.MaxValue)] int pagina = 1, [FromQuery, Range(1, 50)] int quantidade = 5)
        {
            var Jogos = await _jogoService.Obter(pagina, quantidade);

            if (Jogos.Count() == 0)
            {
                return NoContent();
            }

            return Ok(Jogos);
        }

        [HttpGet("{idJogo:guid}")]
        public async Task<ActionResult<JogoViewModel>> Obter([FromRoute]Guid idJogo)
        {
            var jogo = await _jogoService.Obter(idJogo);

            
            if (jogo ==null)
            {
                return NoContent();
            }

            return Ok(jogo);

        }

        [HttpPost]
        public async Task<ActionResult<JogoViewModel>> InserirJogo([FromBody]JogoInputModel jogoInputMoodel)
        {
            try
            {
                var jogo = await _jogoService.Inserir(jogoInputMoodel);
                return Ok(jogo);
            }
           // catch (JogoJaCadastradoException ex)
           catch(Exception ex)
            {

                return UnprocessableEntity("Já existe esse jogo para essa produtora");
            }


        }

        [HttpPut("{idJogo:guid}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute]Guid idJogo, [FromBody]  JogoInputModel jogoInputModel)
        {
            try
            {
                await _jogoService.Atualizar(idJogo, jogoInputModel);
                return Ok();
            }
            // catch (JogoJaCadastradoException ex)
            catch (Exception ex)
            {

                return NotFound("Não existe este jogo");
            }
            

        }


        [HttpPatch("{idJogo:guid}/preco/{preco:double}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute]Guid idJogo, [FromRoute]double preco)
        {
            try
            {
                await _jogoService.Atualizar(idJogo, preco);
                return Ok();
            }
            // catch (JogoJaCadastradoException ex)
            catch (Exception ex)
            {

                return NotFound("Não existe este jogo");
            }

        }
        [HttpDelete("{idJogo:guid}")]
        public async Task<ActionResult> ApagarJogo([FromRoute] Guid idJogo)
        {
            try
            {
                await _jogoService.Remover(idJogo);
                return Ok();
            }
            // catch (JogoJaCadastradoException ex)
            catch (Exception ex)
            {

                return NotFound("Não existe este jogo");
            }

        }


    }
}

