using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace WebScrappingTest
{
    public class CommanderRepository
    {
        private MongoClient _client;
        private IMongoDatabase _db;

        public CommanderRepository(IConfiguration configuration)
        {
            _client = new MongoClient(configuration.GetConnectionString("BancoDeDados"));
            _db = _client.GetDatabase("EDHREC");
        }

        public void Incluir(List<Commander> cards)
        {
            _db.DropCollection("Commander");
            var deckList = _db.GetCollection<Commander>("Commander");
            deckList.InsertMany(cards);
        }
    }
}
