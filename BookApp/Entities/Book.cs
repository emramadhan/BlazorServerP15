using System;
using System.ComponentModel.DataAnnotations;
namespace BookApp.Entities
{
   public class Book
   {
      [Key]
      public long? ISBN { get; set; }
      public string Title { get; set; }
      public short? PubYear { get; set; }
      public DateTime PurchDate { get; set; }
      public int PubId { get; set; }
   }
}