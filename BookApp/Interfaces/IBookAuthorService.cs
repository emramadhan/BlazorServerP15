using BookApp.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookApp.Interfaces
{
   public interface IBookAuthorService
   {
      Task<int> Create(BookAuthorName bookAuthorNamer);
      Task<int> Delete(long isbn, int authorId);
      Task<List<BookAuthorName>> ListAll(long isbn);
   }
}