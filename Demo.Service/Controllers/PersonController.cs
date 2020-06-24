using Demo.Service.Business;
using Microsoft.Extensions.Configuration;

namespace Demo.Service.Api.Controllers
{
    public partial class PersonController
    {
        public PersonController(IPersonManager manager, IConfiguration configuration) : this(manager)
        {
        }
    }
}
