using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Events.Register;

//O ideal é criar uma classe para cada coisa que vai ser feita no código.
//Aqui é definida a regra de negócio
public class RegisterEventsUseCase
{
    public ResponseRegisterEventsJson Execute(RequestEventJson request)
    {
        Validate(request);

        var DbContext = new PassInDbContext();
        var entity = new Infrastructure.Entities.Event
        {
            Title = request.Title,
            Details = request.Details,
            Maximum_Attendees = request.MaximumAttendees,
            //retorna o conteudo do titulo em minuscula e coloca um - sempre que tiver um espaço em branco            
            Slug = request.Title.ToLower().Replace(" ","-"),

        };

        

        DbContext.Events.Add(entity);
        DbContext.SaveChanges();

        return new ResponseRegisterEventsJson
        {
            Id = entity.Id,
        };
    }

    public void Validate(RequestEventJson request) //validação das informações recebidas na request
    {
        if(request.MaximumAttendees <= 0)
        {
            throw new PassInException("Numero de participantes é inválido");
        }

        if(string.IsNullOrWhiteSpace(request.Title))
        {
            throw new PassInException("O titulo não pode ser vazio");
        }
        if (string.IsNullOrWhiteSpace(request.Details))
        {
            throw new PassInException("A descrição não pode ser vazio");
        }
    }
}
