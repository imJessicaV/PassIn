﻿using PassIn.Communication.Responses;
using PassIn.Exceptions;
using PassIn.Infrastructure;
using PassIn.Infrastructure.Entities;

namespace PassIn.Application.UseCases.CheckIns.DoCheckIn;
public class DoCheckInUsecase
{

    private readonly PassInDbContext _dbContext;

    public DoCheckInUsecase()
    {
        _dbContext = new PassInDbContext();
    }
    public ResponseRegisteredJson Execute(Guid attendeeId) 
    {
        Validate(attendeeId);

        var entity = new CheckIn
        {
            Attendee_Id = attendeeId,
            Created_at = DateTime.UtcNow,
        };

        _dbContext.CheckIns.Add(entity);
        _dbContext.SaveChanges();   

        return new ResponseRegisteredJson
        {
            Id = entity.Id, 
        }; 
    }

    private void Validate(Guid attendeeId)
    {
        var existAttendee = _dbContext.Attendees.Any(attendee => attendee.Id == attendeeId);

        if (existAttendee == false)
        {
            throw new NotFoundException("Participante não existe");
        }

        var existCheckIn = _dbContext.CheckIns.Any(ch => ch.Attendee_Id == attendeeId);
        if (existCheckIn)
        {
            throw new ConflictException("Participante já realizou o checkin ");
        }
    }
}
