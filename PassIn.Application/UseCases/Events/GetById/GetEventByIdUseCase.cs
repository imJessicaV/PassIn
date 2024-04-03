using Microsoft.EntityFrameworkCore;
using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;

namespace PassIn.Application.UseCases.Events.GetById;
public class GetEventByIdUseCase
{
    public ResponseEventJson Execute(Guid id)
    {
        var dbContext = new PassInDbContext();

        //dbContext.Events.FirstOrDefault(ev => ev.Id == id); Usado para situações mais complexas quando se quer trazer os dados
        var entity = dbContext.Events.Include(ev => ev.attendees).FirstOrDefault(ev => ev.Id == id);

        if(entity is null)      
            throw new NotFoundException("Id não existe");

        return new ResponseEventJson
        {
            Id = entity.Id,
            Title = entity.Title,
            Details = entity.Details,
            MaximumAttendees = entity.Maximum_Attendees,
            AttendeesAmount = entity.attendees.Count(),

        };
    }
}
