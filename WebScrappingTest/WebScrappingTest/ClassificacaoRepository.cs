using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace WebScrappingTest
{
    public class ClassificacaoRepository
    {
        private MongoClient _client;
        private IMongoDatabase _db;

        public ClassificacaoRepository(IConfiguration configuration)
        {
            _client = new MongoClient(configuration.GetConnectionString("BancoDeDados"));
            _db = _client.GetDatabase("EDHREC");
        }

        public void Incluir(List<Commander> conferencias)
        {
            _db.DropCollection("Commander");
            var deckList = _db.GetCollection<Commander>("Commander");
            deckList.InsertMany(conferencias);
        }
    }
}
