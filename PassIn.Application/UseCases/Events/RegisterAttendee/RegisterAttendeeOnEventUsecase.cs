using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using PassIn.Communication.Requests;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using System.Net.Mail;

namespace PassIn.Application.UseCases.Events.RegisterAttendee;
public class RegisterAttendeeOnEventUsecase
{
    private readonly PassInDbContext _dbContext;
    public RegisterAttendeeOnEventUsecase()
    {
        _dbContext = new PassInDbContext();
    }
    public void Execute(Guid eventId,  RequestRegisterEventJson request)
    {
        Validate(eventId, request);

        var entity = new Infrastructure.Entities.Attendee
        {
            Email = request.Email,
            Name = request.Name,
            Event_Id = eventId,
            Created_at = DateTime.UtcNow,
        };

        _dbContext.Attendees.Add(entity);
        _dbContext.SaveChanges();
        

    }

    private void Validate(Guid eventId, RequestRegisterEventJson request)
    {
        var eventExist = _dbContext.Events.Any(ev => ev.Id == eventId);

        if (eventExist == false)
            throw new NotFoundException("Id do evento não existe");

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ErrorOnValidationException("Nome inválido");

        }
        var emailIsValid = EmailIsValide(request.Email);
        if (emailIsValid == false)
        {
            throw new ErrorOnValidationException("E-mail inválido");
        }

        var attendeeAlredyRegistered = _dbContext.Attendees.Any(attendee => attendee.Email.Equals(request.Email) && attendee.Event_Id == eventId);
        if (attendeeAlredyRegistered)
        {
            throw new ErrorOnValidationException("Você já está registrado no evento informado");
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
