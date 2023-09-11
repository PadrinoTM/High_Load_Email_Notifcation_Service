namespace ReventTask.API.Controllers.v1;
[Route("api/v{version:apiVersion}/[controller]")]
//[Route("api/[controller]")]
[ApiController]
[ApiVersion("1.0")]
public class BaseController : ControllerBase
{

    public BaseController()
    {
      
    }
 
  internal Error PopulateError (int code, string message, string type)
   {
       return new Error(){
           Code = code,
           Message = message,
           Type = type
       };
   }
}
