﻿using Demo.Service.Business.Data;
using Microsoft.Extensions.Configuration;

namespace Demo.Service.Business.DataSvc
{
    public partial class PersonDataSvc
    {
        public PersonDataSvc(IPersonData data, IConfiguration configuration) : this(data)
        {
        }
    }
}
