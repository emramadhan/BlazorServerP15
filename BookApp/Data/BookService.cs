using BookApp.Interfaces;
using BookApp.Entities;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BookApp.Data
{
   public class BookService : IBookService
   {
      private readonly IDapperService _dapperService;

      public BookService(IDapperService dapperService)
      {
         this._dapperService = dapperService;
      }

      public Task<long> Create(Book book)
      {
         var dbPara = new DynamicParameters();
         dbPara.Add("ISBN", book.ISBN, DbType.Int64);
         dbPara.Add("Title", book.Title, DbType.String);
         dbPara.Add("PubYear", book.PubYear, DbType.Int16);
         dbPara.Add("PurchDate", book.PurchDate, DbType.Date);
         dbPara.Add("PubId", book.PubId, DbType.Int32);
         var bookId = Task.FromResult(_dapperService.
             Insert<long>("[dbo].[spAddBook]", dbPara, 
             commandType: CommandType.StoredProcedure));
         return bookId;
      }

      public Task<Book> ReadByPk(long isbn)
      {
         var book = Task.FromResult(_dapperService.Get<Book>
             ($"select * from [Book] where ISBN = {isbn}",
             null, commandType: CommandType.Text));
         return book;
      }

      public Task<int> Update(Book book, long pk)
      {
         var dbPara = new DynamicParameters();
         dbPara.Add("ISBN", book.ISBN, DbType.Int64);
         dbPara.Add("Title", book.Title, DbType.String);
         dbPara.Add("PubYear", book.PubYear, DbType.Int16);
         dbPara.Add("PurchDate", book.PurchDate, DbType.Date);
         dbPara.Add("PubId", book.PubId, DbType.Int32);
         dbPara.Add("Pk", pk, DbType.Int64);

         var updateBook = Task.FromResult(_dapperService.
             Update<int>("[dbo].[spUpdateBook]", dbPara, 
             commandType: CommandType.StoredProcedure));
         return updateBook;
      }

      public Task<int> Delete(long id)
      {
         var deleteBook = Task.FromResult(_dapperService.Execute
             ($"Delete [Book] where ISBN = {id}", null, 
             commandType: CommandType.Text));
         return deleteBook;
      }

      public Task<int> Count(string search)
      {
         var totBook = Task.FromResult(_dapperService.Get<int>
            ($"SELECT COUNT(*) FROM Book B {search}", null, 
            commandType: CommandType.Text));
         return totBook;
      }

      public Task<List<BookAuPub>> ListAll(int skip,
             int take, string orderBy, string direction = "DESC",
             string search = "")
      {
         var books = Task.FromResult(_dapperService.
         GetAll<BookAuPub>($"SELECT B.ISBN,Title, " +
         $"FName + ' ' + LName AuthorName, PubYear, PurchDate, " +
         $"P.Name PubName FROM Book B LEFT OUTER JOIN" +
         $" Publisher P ON B.PubId = P.Id LEFT OUTER JOIN " +
         $"BookAuthor BA ON B.ISBN = BA.ISBN LEFT OUTER JOIN " +
         $"Author A ON BA.AuthorId = A.Id {search} " +
         $"ORDER BY {orderBy} {direction} OFFSET {skip} " +
         $"ROWS FETCH NEXT {take} ROWS ONLY;", null, 
         commandType: CommandType.Text));
         return books;
      }
   }
}