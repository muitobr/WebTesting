using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace WebScrappingTest
{
    public class Commander
    {
        public ObjectId _id { get; set; }
        public string Comandante { get; set; }
        public DateTime DataCarga { get; set; }
        public List<Cartas> Cards { get; set; } = new List<Cartas>();
    }

    public class Cartas
    {
        public int Posicao { get; set; }
        public string Nome { get; set; }
        public string percentualDecks { get; set; }
        public string percentualSinergia { get; set; }
    }
}