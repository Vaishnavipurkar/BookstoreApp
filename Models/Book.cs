using System;
using System.ComponentModel.DataAnnotations;
/// <summary>
/// Summary description for Class1
/// </summary>
namespace BookstoreApp.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Author { get; set; }

        public string Genre { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }
        public int Stock { get; set; }// for buying the book

        public Book()
        {
            // Initialize Author (e.g., set it to an empty string)
            Title = string.Empty;
            Author = string.Empty;
            Genre = string.Empty;
            Price = 0;
        }
    }
}
