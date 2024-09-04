using System;
using System.ComponentModel.DataAnnotations;

namespace BookstoreApp.Models
{
    public class Purchase
    {
        public int Id { get; set; }

        [Required]
        public int BookId { get; set; }

        public Book Book { get; set; }

        [Required]
        [StringLength(100)]
        public string BuyerName { get; set; }

        public int Quantity { get; set; }

        public DateTime PurchaseDate { get; set; }

        public decimal TotalPrice { get; set; }

        public bool IsPaid { get; set; }
    }
}
