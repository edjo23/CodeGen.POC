using Demo.Service.Business.DataSvc;
using Microsoft.Extensions.Configuration;

namespace Demo.Service.Business
{
    public partial class PersonManager
    {
        public PersonManager(IPersonDataSvc dataService, IConfiguration configuration) : this(dataService)
        {                
        }
    }
}
