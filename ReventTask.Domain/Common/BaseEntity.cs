namespace ReventTask.Domain.Common;
public abstract class BaseEntity
{
   public string CreatedBy { get; set;}
   public DateTime DateCreated { get; set;}
   public DateTime TimeCreated { get; set;}
   public string UpdatedBy { get; set;}
   public DateTime DateUpdated { get; set;}
   public DateTime TimeUpdated { get; set;}
   public string ApprovedBy { get; set;}
   public DateTime DateApproved { get; set;}
   public DateTime TimeApproved { get; set;}
   public string Status { get; set;}
   public string DeletedFlag { get; set;}
   public string HashValue { get; set;}
   public Int64 Version { get; set;}

}

