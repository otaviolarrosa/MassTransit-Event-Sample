using MongoDB.Driver;

namespace Data
{
    public class Connection
    {
        
        private IMongoCollection<Entities.Log> _collection;
        public IMongoCollection<Entities.Log> GetCollection()
        {
            if (_collection == null)
            {
                IMongoClient client = new MongoClient(Constants.MongoDbConnectionString);
                IMongoDatabase database = client.GetDatabase("dbLogs");
                _collection = database.GetCollection<Entities.Log>("Log");
            }
            return _collection;
        }
    }
}
