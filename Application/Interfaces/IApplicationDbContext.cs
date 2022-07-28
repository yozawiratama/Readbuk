using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Author> Authors { get; set; }
        DbSet<Book> Books { get; set; }
        DbSet<BookAuthor> BookAuthors { get; set; }
        DbSet<BookVariant> BookVariants { get; set; }
        DbSet<Borrowing> Borrowings { get; set; }
        DbSet<Publisher> Publishers { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Language> Languages { get; set; }
        Task<int> SaveChangesAsync();
    }
}
