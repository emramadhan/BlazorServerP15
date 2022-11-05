using BookApp.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BookApp.Interfaces
{
   public interface IBookService
   {
      Task<long> Create(Book book);
      Task<Book> ReadByPk(long isbn);
      Task<int> Update(Book book, long pk);
      Task<int> Delete(long id);
      Task<int> Count(string search);
      Task<List<BookAuPub>> ListAll(int skip,
                                    int take,
                                    string orderBy,
                                    string direction,
                                    string search);
   }
}