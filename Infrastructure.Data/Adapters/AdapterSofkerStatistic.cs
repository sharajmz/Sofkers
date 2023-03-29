using Application.Interfaces;
using Domain.DTOs;
using Domain.Entities;
using Infrastructure.Data.Context;
using Newtonsoft.Json;

namespace Infrastructure.Data.Adapters
{
    public class AdapterSofkerStatistic : IAdapterSofkerStatistic
    {
        private readonly SofkaStatisticsDbContext _context;
        public AdapterSofkerStatistic(SofkaStatisticsDbContext context) 
        {
            _context = context;
        }
        public async Task<bool> InsertSofkertChangesLog(List<string> sofkerStatisticMongo)
        {
            var collection = _context.SofkerStatisticCollection;
            
            if (sofkerStatisticMongo.Count > 0)
            {
                foreach (var item in sofkerStatisticMongo)
                {
                    SofkerStatisticMongoDb sofkerStatisticMongoDb = JsonConvert.DeserializeObject<SofkerStatisticMongoDb>(item);
                    sofkerStatisticMongoDb.id = DateTime.Now.Ticks.ToString();
                    try
                    {
                        await collection.InsertOneAsync(sofkerStatisticMongoDb);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
