using System.ComponentModel.DataAnnotations;

namespace BookApp.Entities
{
   public class Author
   {
      [Key]
      public int Id { get; set; }
      public string FName { get; set; }
      public string LName { get; set; }
      public string Phone { get; set; }
   }
}