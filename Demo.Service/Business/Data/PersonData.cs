using Microsoft.Extensions.Configuration;

namespace Demo.Service.Business.Data
{
    public partial class PersonData
    {
        public PersonData(IDataStorage dataStorage, IConfiguration configuration) : this(dataStorage)
        {
        }
    }
}
