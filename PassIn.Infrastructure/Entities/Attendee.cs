﻿using System.ComponentModel.DataAnnotations.Schema;

namespace PassIn.Infrastructure.Entities;
public class Attendee
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty ;
    public Guid Event_Id { get; set; }
    public DateTime Created_at { get; set; }
    public CheckIn? CheckIns { get; set; } = [];
}
