using System.Web.Http;

namespace EduCon.Api.Controllers
{
    public class StubController : ApiController
    {
        [HttpGet]
        public string Echo(string text)
        {
            return text;
        }
    }
}