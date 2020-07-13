using Beef.AspNetCore.WebApi;
using Demo.Service.Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Demo.Service.Api.Controllers
{
    public partial class PersonController
    {
        public PersonController(IPersonManager manager, IConfiguration configuration) : this(manager)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;

        [HttpGet]
        [Route("config/{key}")]
        public IActionResult GetConfig(string key)
        {
            return new OkObjectResult(JsonConvert.SerializeObject(_configuration.GetValue<string>(key)));
        }
    }
}
