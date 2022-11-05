using System.ComponentModel.DataAnnotations;

namespace BookApp.Entities
{
   public class BookAuthor
   {
      [Key]
      public long ISBN { get; set; }
      public int AuthorId { get; set; }
      public byte? AuthorOrd { get; set; }
   }
}