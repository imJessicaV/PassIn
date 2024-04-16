﻿using Microsoft.EntityFrameworkCore;
using PassIn.Infrastructure.Entities;

namespace PassIn.Infrastructure;
public class PassInDbContext : DbContext
{
    public DbSet<Event> Events { get; set; }
    public DbSet<Attendee> Attendees { get; set; }

    public DbSet<CheckIn> CheckIns { get; set; }   

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=C:\\xampp\\DB\\PassInDb.db");
    }

    public static implicit operator int(PassInDbContext v)
    {
        throw new NotImplementedException();
    }
}

//teste teste
