namespace ReventTask.Domain;
public partial class InboundLogDto
{
    public string RequestSystem { get; set;}
    public string APICalled { get; set;}
    public string APIMethod { get; set;}
    public DateTime LogDate { get; set;}
    public string ImpactUniqueIdentifier { get; set;}
    public string ImpactUniqueidentifierValue { get; set;}
    public string AlternateUniqueIdentifier { get; set;}
    public string AlternateUniqueidentifierValue { get; set;}
    public string RequestDetails { get; set;}
    public DateTime RequestDateTime { get; set;}
    public string ResponseDetails { get; set;}
    public DateTime ResponseDateTime { get; set;}
    public List<OutboundLog> OutboundLogs { get; set;}
    public string ExceptionDetails { get; set;}

}

