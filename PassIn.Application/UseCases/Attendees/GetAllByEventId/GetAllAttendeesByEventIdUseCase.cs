using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;

namespace PassIn.Application.UseCases.Attendees.GetAllByEventId;
public class GetAllAttendeesByEventIdUseCase
{
    private readonly PassInDbContext _dbContext;
    public GetAllAttendeesByEventIdUseCase()
    {
        _dbContext = new PassInDbContext();
    }
    public ResponseAllAttendeesJson Execute(Guid eventId)
    {
        var entity = _dbContext.Events.Include(ev => ev.attendees).ThenInclude(attendee => attendee.CheckIns).FirstOrDefault(ev => ev.Id == eventId);

        if (entity is null)
            throw new NotFoundException("Id não existe");

        return new ResponseAllAttendeesJson
        {
            Attendees = entity.attendees.Select(Attendee => new ResponseAttendeeJson
            {
                Id = eventId,   
                Name = Attendee.Name,
                Email = Attendee.Email,
                CreatedAt = DateTime.UtcNow,
                CheckedInAt = Attendee.CheckIns?.Created_at
            }).ToList()
        };
    }
}
