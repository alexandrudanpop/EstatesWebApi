namespace Api.Model
{
    using AppBoot;
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
    using System;

    public class MongoDbContext<T> where T : class
    {
        public IMongoCollection<T> Collection { get; private set;  }

        public MongoDbContext(IOptions<AppConfig> config)
        {
            string connectionString = config.Value.ConnectionString;
            MongoClient mongoClient = new MongoClient(connectionString);
            var db = mongoClient.GetDatabase("estates");

            InitCollection(db);
        }

        private void InitCollection(IMongoDatabase db)
        {
            var collectionName = Activator.CreateInstance(typeof(T)).ToString().ToLower();
            Collection = db.GetCollection<T>(collectionName);
        }
    }
}