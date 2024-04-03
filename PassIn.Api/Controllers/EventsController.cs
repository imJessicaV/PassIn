using Microsoft.AspNetCore.Mvc;
using PassIn.Application.UseCases.Events.GetById;
using PassIn.Application.UseCases.Events.Register;
using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;

namespace PassIn.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterEventsJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public IActionResult Register([FromBody] RequestEventJson request)
    {
        try //tenta executar o que estiver nas {}
        {
            var useCase = new RegisterEventsUseCase();

            var response = useCase.Execute(request); //executa a regra de negócio

            return Created(string.Empty, response);

        //Tratamentos das mensagens de erro
        }
        catch (PassInException ex) // cai aqui se der algum tipo de exceção
        {
            return BadRequest(new ResponseErrorJson(ex.Message));
        }
        catch
        {
            //mostra o erro especifico
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorJson("Erro desconhecido"));
         
        }
    }

    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(typeof(ResponseEventJson), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status404NotFound)]
    public IActionResult GetById([FromRoute] Guid id)
    {

        try
        {
            var useCase = new GetEventByIdUseCase();
            var response = useCase.Execute(id);

            return Ok(response);
        }
        catch (PassInException ex)
        {
            return NotFound(new ResponseErrorJson(ex.Message));
        }
        catch
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new ResponseErrorJson("Erro desconhecido"));
        }
    }
}
