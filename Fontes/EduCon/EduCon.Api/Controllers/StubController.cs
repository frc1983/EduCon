using System.Web.Http;

namespace EduCon.Api.Controllers
{
    [RoutePrefix("api/v1")]
    public class StubController : ApiController
    {
        [Route("echo")]
        [HttpGet]
        public string Echo(string text)
        {
            return text;
        }
    }
}