namespace ReventTask.Core.Data;

public partial interface IReventDBContext
{
    //IMongoCollection<OutboundLog> OutboundLogs { get; set; }
     IMongoCollection<InboundLog> InboundLogs { get; set; }
 
}

