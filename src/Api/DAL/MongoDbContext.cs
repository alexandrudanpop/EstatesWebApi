namespace Api.DAL
{
    using System;

    using Api.AppBoot;

    using Microsoft.Extensions.Options;

    using MongoDB.Driver;

    public class MongoDbContext<T>
        where T : class
    {
        public MongoDbContext(IOptions<AppConfig> config)
        {
            var connectionString = config.Value.ConnectionString;
            var mongoClient = new MongoClient(connectionString);
            var db = mongoClient.GetDatabase("estates");

            this.InitCollection(db);
        }

        public IMongoCollection<T> Collection { get; private set; }

        private void InitCollection(IMongoDatabase db)
        {
            var collectionName = Activator.CreateInstance(typeof(T)).ToString().ToLower();
            this.Collection = db.GetCollection<T>(collectionName);
        }
    }
}