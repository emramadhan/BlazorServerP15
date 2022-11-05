using BookApp.Interfaces;
using BookApp.Entities;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BookApp.Data
{
   public class AuthorService : IAuthorService
   {
      private readonly IDapperService _dapperService;

      public AuthorService(IDapperService dapperService)
      {
         this._dapperService = dapperService;
      }

      public Task<int> Create(Author author)
      {
         var dbPara = new DynamicParameters();
         dbPara.Add("FName", author.FName, DbType.String);
         dbPara.Add("LName", author.LName, DbType.String);
         dbPara.Add("Phone", author.Phone, DbType.String);
         var authorId = Task.FromResult(_dapperService.Insert<int>
             ("[dbo].[spAddAuthor]", dbPara, 
             commandType: CommandType.StoredProcedure));
         return authorId;
      }

      public Task<Author> ReadByPk(int id)
      {
         var author = Task.FromResult(_dapperService.Get<Author>
             ($"select * from [Author] where Id = {id}", null,
             commandType: CommandType.Text));
         return author;
      }

      public Task<int> Update(Author author)
      {
         var dbPara = new DynamicParameters();
         dbPara.Add("Id", author.Id);
         dbPara.Add("FName", author.FName, DbType.String);
         dbPara.Add("LName", author.LName, DbType.String);
         dbPara.Add("Phone", author.Phone, DbType.String);
         var updateAuthor = Task.FromResult(_dapperService.
             Update<int>("[dbo].[spUpdateAuthor]", dbPara, 
             commandType: CommandType.StoredProcedure));
         return updateAuthor;
      }

      public Task<int> Delete(int id)
      {
         var deleteAuthor = Task.FromResult(_dapperService.Execute
             ($"Delete [Author] where Id = {id}", null,
             commandType: CommandType.Text));
         return deleteAuthor;
      }

      public Task<int> Count(string search)
      {
         var totAuthor = Task.FromResult(_dapperService.Get<int>
             ($"SELECT COUNT(*) FROM [Author] WHERE LName like " +
             $"'%{search}%'", null, commandType: CommandType.Text));
         return totAuthor;
      }

      public Task<List<Author>> ListAll()
      {
         var authors = Task.FromResult(_dapperService.GetAll<Author>
             ($"SELECT * FROM [Author] ORDER BY FName; ", null,
             commandType: CommandType.Text));         
         return authors;
      }

      public Task<List<Author>> ListAll(int skip, int take,
             string orderBy, string direction = "DESC", 
             string search = "")
      {
         var authors = Task.FromResult(_dapperService.GetAll<Author>
             ($"SELECT * FROM [Author] WHERE LName like " +
             $"'%{search}%' ORDER BY {orderBy} {direction} " +
             $"OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY; ",
             null, commandType: CommandType.Text));
         return authors;
      }
   }
}