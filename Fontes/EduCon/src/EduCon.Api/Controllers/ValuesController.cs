using System.Collections.Generic;
using Microsoft.AspNet.Mvc;
using Swashbuckle.SwaggerGen.Annotations;

namespace EduCon.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ValuesController : Controller
    {
        // GET: api/values
        [HttpGet]
        [SwaggerResponse(System.Net.HttpStatusCode.OK)]
        public IEnumerable<Values> Get()
        {
            return new Values[] { new Values { Value1 = "value1", Value2 = "value2" } };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    public class Values
    {
        public string Value1 { get; set; }
        public string Value2 { get; set; }
    }
}