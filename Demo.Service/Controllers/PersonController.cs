using Demo.Service.Business;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Service.Api.Controllers
{
    public partial class PersonController
    {
        public PersonController(IPersonManager manager, IConfiguration configuration) : this(manager)
        {
        }
    }
}
