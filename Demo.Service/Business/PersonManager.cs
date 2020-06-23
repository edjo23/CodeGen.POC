using Demo.Service.Business.DataSvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Service.Business
{
    public partial class PersonManager
    {
        public PersonManager(IPersonDataSvc dataService, IConfiguration configuration) : this(dataService)
        {                
        }
    }
}
