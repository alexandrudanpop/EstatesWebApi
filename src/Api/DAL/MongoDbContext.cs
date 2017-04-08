namespace Api.DAL
{
    using System;

    using Api.AppBoot;

    using Microsoft.Extensions.Options;

    using MongoDB.Driver;

    public class MongoDbContext<T> where T : class
    {
        public IMongoCollection<T> Collection { get; private set;  }

        public MongoDbContext(IOptions<AppConfig> config)
        {
            string connectionString = config.Value.ConnectionString;
            MongoClient mongoClient = new MongoClient(connectionString);
            var db = mongoClient.GetDatabase("estates");

            this.InitCollection(db);
        }

        private void InitCollection(IMongoDatabase db)
        {
            var collectionName = Activator.CreateInstance(typeof(T)).ToString().ToLower();
            this.Collection = db.GetCollection<T>(collectionName);
        }
    }
}