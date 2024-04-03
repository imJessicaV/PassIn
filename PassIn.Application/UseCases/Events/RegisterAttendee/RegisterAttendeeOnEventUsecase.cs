using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Options;
using PassIn.Communication.Requests;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;
using System.Net.Mail;

namespace PassIn.Application.UseCases.Events.RegisterAttendee;
public class RegisterAttendeeOnEventUsecase
{
    private readonly PassInDbContext _dbContext;
    public RegisterAttendeeOnEventUsecase()
    {
        _dbContext = new PassInDbContext();
    }
    public ResponseRegisteredJson Execute(Guid eventId,  RequestRegisterEventJson request)
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

        return new ResponseRegisteredJson
        {
            Id = entity.Id,
        };

    }

    private void Validate(Guid eventId, RequestRegisterEventJson request)
    {
        var eventEntity = _dbContext.Events.Find(eventId);

        if (eventEntity is null)
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
            throw new ConflictException("Você já está registrado no evento informado");
        }

        var attendeeForEvent = _dbContext.Attendees.Count(attendee => attendee.Event_Id == eventId);
        if (attendeeForEvent == eventEntity.Maximum_Attendees)
        {
            throw new ErrorOnValidationException("Não há mais vagas para esse evento");
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
