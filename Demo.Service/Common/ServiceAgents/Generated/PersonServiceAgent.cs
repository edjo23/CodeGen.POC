#nullable enable

using Beef.WebApi;
using Demo.Service.Common.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Demo.Service.Common.ServiceAgents
{
    public partial class PersonServiceAgent : WebApiServiceAgentBase<PersonServiceAgent>
    {
        public PersonServiceAgent(IOptionsMonitor<ServiceAgentOptions> namedOptionsAccessor)
            : base(namedOptionsAccessor.Get(typeof(PersonServiceAgent).FullName).HttpClient, namedOptionsAccessor.Get(nameof(PersonServiceAgent)).BeforeRequest) { }

    }
}

#nullable restore