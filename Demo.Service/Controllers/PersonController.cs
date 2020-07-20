using Beef.AspNetCore.WebApi;
using Demo.Service.Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Demo.Service.Api.Controllers
{
    public partial class PersonController
    {
        public PersonController(IPersonManager manager, IConfiguration configuration, ILogger<PersonController> logger) : this(manager)
        {
            _configuration = configuration;
            _logger = logger;
        }

        private readonly IConfiguration _configuration;
        private readonly ILogger<PersonController> _logger;

        [HttpGet]
        [Route("config/{key}")]
        public IActionResult GetConfig(string key)
        {
            _logger.LogInformation($"Executing {nameof(GetConfig)}({key})...");
            return new OkObjectResult(JsonConvert.SerializeObject(_configuration.GetValue<string>(key)));
        }
    }
}
