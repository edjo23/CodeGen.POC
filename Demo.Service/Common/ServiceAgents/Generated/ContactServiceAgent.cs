#nullable enable

using Beef.WebApi;
using Demo.Service.Common.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Demo.Service.Common.ServiceAgents
{
    public class ContactServiceAgent : WebApiServiceAgentBase<ContactServiceAgent>
    {
        public ContactServiceAgent(IOptionsMonitor<ServiceAgentOptions> namedOptionsAccessor)
            : base(namedOptionsAccessor.Get(typeof(ContactServiceAgent).FullName).HttpClient, namedOptionsAccessor.Get(nameof(ContactServiceAgent)).BeforeRequest) { }

        public async Task<WebApiAgentResult<ContactCollection>> GetCollAsync()
        {
            return await base.GetAsync<ContactCollection>("api/v1/contacts/", requestOptions: null,
                args: Array.Empty<WebApiArg>());
        }
    }
}

#nullable restore