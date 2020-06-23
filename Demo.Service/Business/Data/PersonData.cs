using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Service.Business.Data
{
    public partial class PersonData
    {
        public PersonData(IDataStorage dataStorage, IConfiguration configuration) : this(dataStorage)
        {
        }
    }
}
