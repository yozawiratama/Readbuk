using System;
using Application.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Interfaces
{
    public interface IIdentityDbContext
    {
        DbSet<ApplicationUser> Users { get; set; }
    }
}

