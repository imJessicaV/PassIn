using Microsoft.EntityFrameworkCore.ChangeTracking;
using PassIn.Communication.Requests;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using System.Net.Mail;

namespace PassIn.Application.UseCases.Events.RegisterAttendee;
public class RegisterAttendeeOnEventUsecase
{
    public void Execute(Guid eventId,  RequestEventJson request)
    {
        var dbContext = new PassInDbContext();


    }

    private void Validade(Guid eventId, RequestRegisterEventJson request, PassInDbContext dbContext)
    {
        var eventExist = dbContext.Events.Any(ev => ev.Id == eventId);

        if (eventExist == false)
            throw new NotFoundException("Id do evento não existe");

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ErrorOnValidationException("Nome inválido");

        }
    }

    private bool EmailIsValide(string email)
    {
        try
        {
            new MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
