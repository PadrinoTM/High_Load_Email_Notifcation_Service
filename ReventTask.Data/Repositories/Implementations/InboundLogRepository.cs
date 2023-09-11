namespace ReventTask.Core.Repositories;
public partial class InboundLogRepository : IInboundLogRepository
{

    private readonly IReventDBContext context;
    private readonly ILogger logger;



    public InboundLogRepository(IReventDBContext context, ILogger logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<string> CreateInboundLog(InboundLog inboundLog)
    {

        try
        {
            await context.InboundLogs.InsertOneAsync(inboundLog);
            return inboundLog.InboundLogId;
        }
        catch (Exception)
        {
            return string.Empty;
        }

    }

    public async Task<List<InboundLog>> GetInboundLogs()
    {
        var results = await context.InboundLogs.FindAsync(_ => true);
        return results.ToList();

    }

    public async Task<InboundLog> GetInboundLog(string inboundLogId)
    {
        var filter = Builders<InboundLog>.Filter.Eq(m => m.InboundLogId, inboundLogId);
        var inboundLog = await context.InboundLogs.Find(filter).FirstOrDefaultAsync();

        return inboundLog;

    }

    //GetList by any Field Name template. Uncomment If Needed. Remember to add to your IInboundLogRepository.cs
    /* public async Task<List<InboundLog>> GetByFieldName(string fieldName)
    {
        var filter = Builders<InboundLog>.Filter.Eq(m => m.fieldName, fieldName);
        var inboundLogs =await context.InboundLogs.Find(filter).ToList();

        return inboundLogs ;
    } */

    public async Task<bool> RemoveInboundLog(string inboundLogId)
    {
        var filter = Builders<InboundLog>.Filter.Eq(m => m.InboundLogId, inboundLogId);
        var result = await context.InboundLogs.DeleteOneAsync(filter);

        return result.DeletedCount == 1;

    }

    public async Task<bool> UpdateInboundLog(string id, InboundLog inboundLog)
    {
        var result = await context.InboundLogs.ReplaceOneAsync(inboundLog => inboundLog.InboundLogId == id, inboundLog);
        return result.ModifiedCount == 1;
    }

    //Use Template To Update Specific Fields According to Needs. Remember to add to your IInboundLogRepository.cs
    /*
    public async Task<bool> UpdateSpecificFields(string inboundLogId, InboundLog inboundLog)
    {
        var filter = Builders<InboundLog>.Filter.Eq(m => m.InboundLogId, inboundLogId);
        var update = Builders<InboundLog>.Update
        .Set(m => m.Field1, inboundLog.Field1)
        .Set(m => m.Field2, inboundLog.Field2)
        .Set(m => m.Field3, inboundLog.Field3);
    
        var result = await context.InboundLogs.UpdateOneAsync(filter, update);

        return result.ModifiedCount == 1;
    }
    */
}




