namespace ReventTask.Domain;
public partial class OutboundLogDto
{
    public string SystemCalledName { get; set;}
    public string APICalled { get; set;}
    public string APIMethod { get; set;}
    public DateTime LogDate { get; set;}
    public string RequestDetails { get; set;}
    public DateTime RequestDateTime { get; set;}
    public string ResponseDetails { get; set;}
    public DateTime ResponseDateTime { get; set;}
    public string ExceptionDetails { get; set;}

}

