using BookApp.Interfaces;
using BookApp.Entities;
using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace BookApp.Data
{
   public class PublisherService : IPublisherService
   {
      private readonly IDapperService _dapperService;

      public PublisherService(IDapperService dapperService)
      {
         this._dapperService = dapperService;
      }

      public Task<int> Create(Publisher publisher)
      {
         var dbPara = new DynamicParameters();
         dbPara.Add("Name", publisher.Name, DbType.String);
         dbPara.Add("City", publisher.City, DbType.String);
         dbPara.Add("Country", publisher.Country, DbType.String);
         var publisherId = Task.FromResult(_dapperService.
             Insert<int>("[dbo].[spAddPublisher]", dbPara,
             commandType: CommandType.StoredProcedure));
         return publisherId;
      }

      public Task<Publisher> ReadByPk(int id)
      {
         var publisher = Task.FromResult(_dapperService.Get
         <Publisher>($"select * from [Publisher] where Id = {id}",
         null, commandType: CommandType.Text));
         return publisher;
      }

      public Task<int> Update(Publisher publisher)
      {
         var dbPara = new DynamicParameters();
         dbPara.Add("Id", publisher.Id);
         dbPara.Add("Name", publisher.Name, DbType.String);
         dbPara.Add("City", publisher.City, DbType.String);
         dbPara.Add("Country", publisher.Country, DbType.String);
         var updatePublisher = Task.FromResult(_dapperService.
             Update<int>("[dbo].[spUpdatePublisher]", dbPara, 
             commandType: CommandType.StoredProcedure));
         return updatePublisher;
      }
      public Task<int> Delete(int id)
      {
         var deletePublisher = Task.FromResult(_dapperService.
             Execute($"Delete [Publisher] where Id = {id}", null,
             commandType: CommandType.Text));
         return deletePublisher;
      }

      public Task<int> Count(string search)
      {
         var totPublisher = Task.FromResult(_dapperService.Get<int>
             ($"select COUNT(*) from [Publisher] WHERE Name like " +
             $"'%{search}%'", null, commandType: CommandType.Text));
         return totPublisher;
      }

      public Task<List<Publisher>> ListAll()
      {
         var publishers = Task.FromResult
             (_dapperService.GetAll<Publisher>
             ($"SELECT * FROM [Publisher] ORDER BY Name; ",
              null, commandType: CommandType.Text));
         return publishers;
      }

      public Task<List<Publisher>> ListAll(int skip, int take,
             string orderBy, string direction = "DESC",
             string search = "")
      {
         var publishers = Task.FromResult(_dapperService.GetAll
             <Publisher>($"SELECT * FROM [Publisher] WHERE Name " +
             $"like '%{search}%' ORDER BY {orderBy} {direction} " +
             $"OFFSET {skip} ROWS FETCH NEXT {take} ROWS ONLY; ",
             null, commandType: CommandType.Text));
         return publishers;
      }
   }
}