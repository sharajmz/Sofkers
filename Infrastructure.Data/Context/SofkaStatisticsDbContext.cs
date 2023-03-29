using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Infrastructure.Data.Context
{
    public class SofkaStatisticsDbContext
    {
        private readonly IMongoDatabase _database;
        private readonly IConfiguration _configuration;

        public SofkaStatisticsDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            var _mongoClient = new MongoClient(_configuration.GetSection("ConnectionMongoDb").Value);
            _database = _mongoClient.GetDatabase(_configuration.GetSection("MongoDb").Value);
        }

        public IMongoCollection<SofkerStatisticMongoDb> SofkerStatisticCollection
        {
            get
            {
                return _database.GetCollection<SofkerStatisticMongoDb>(_configuration.GetSection("MongoCollection").Value);
            }
        }
    }
}
