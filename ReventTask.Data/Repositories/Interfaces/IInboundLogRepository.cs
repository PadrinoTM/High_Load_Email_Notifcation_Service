namespace ReventTask.Core.Repositories;
public partial interface IInboundLogRepository
{
    Task<List<InboundLog>> GetInboundLogs();
    Task<InboundLog>  GetInboundLog(string id);
    Task<string> CreateInboundLog(InboundLog inboundLog);
    Task<bool> UpdateInboundLog(string id, InboundLog inboundLog);
    Task<bool> RemoveInboundLog(string id);

    //Task<List<InboundLog>> GetByFieldName(string fieldName) --Template
    //Task<bool> UpdateSpecificFields(string inboundLogId, InboundLog inboundLog) --Template

}
