using Demo.Service.Business.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Service.Business.DataSvc
{
    public partial class PersonDataSvc
    {
        public PersonDataSvc(IPersonData data, IConfiguration configuration) : this(data)
        {
        }
    }
}
