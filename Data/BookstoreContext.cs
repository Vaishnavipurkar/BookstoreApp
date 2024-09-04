using System;
using Microsoft.EntityFrameworkCore;
using BookstoreApp.Models;

/// <summary>
/// Summary description for Class1
/// </summary>

namespace BookstoreApp.Data
{
    public class BookstoreContext : DbContext
    {
        public BookstoreContext(DbContextOptions<BookstoreContext> options)
            : base(options)
        {

        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Purchase> purchases { get; set; } // new dbSet
    }
}

