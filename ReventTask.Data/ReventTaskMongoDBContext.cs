namespace ReventTask.Core.Data;

public partial class ReventDBContext : IReventDBContext
{
      public IMongoCollection<InboundLog> InboundLogs { get; set; }

    public ReventDBContext(IMongoDbConfig config,IMongoClient mongoClient)
    {
        var client = new MongoClient(config.ConnectionString);
        var database = client.GetDatabase(config.DatabaseName);

           InboundLogs = database.GetCollection<InboundLog>("InboundLogs");


    }

}

