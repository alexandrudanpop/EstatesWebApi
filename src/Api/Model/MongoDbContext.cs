namespace Api.Model
{
    using MongoDB.Driver;
    using System;

    public class MongoDbContext<T> where T : class
    {
        private const string DbName = "Estates";

        // todo - should be configurable
        private const string DbUrl = @"mongodb://localhost:27017";

        public IMongoCollection<T> Collection { get; private set;  }

        public MongoDbContext()
        {
            var mongoClient = new MongoClient(DbUrl);
            var db = mongoClient.GetDatabase(DbName);

            InitCollection(db);
        }

        private void InitCollection(IMongoDatabase db)
        {
            var collectionName = Activator.CreateInstance(typeof(T)).ToString();
            Collection = db.GetCollection<T>(collectionName);
        }
    }
}