#nullable enable

using Beef.WebApi;
using Demo.Service.Common.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Demo.Service.Common.ServiceAgents
{
    public partial class ContactServiceAgent : WebApiServiceAgentBase<ContactServiceAgent>
    {
        public ContactServiceAgent(IOptionsMonitor<ServiceAgentOptions> namedOptionsAccessor)
            : base(namedOptionsAccessor.Get(typeof(ContactServiceAgent).FullName).HttpClient, namedOptionsAccessor.Get(nameof(ContactServiceAgent)).BeforeRequest) { }

        public async Task<WebApiAgentResult<Contact?>> GetAsync(Guid id, WebApiRequestOptions? requestOptions = null)
        {
            return await base.GetAsync<Contact?>("api/v1/contacts/{id}", requestOptions: requestOptions,
                args: new WebApiArg[] { new WebApiArg<Guid>("id", id) });
        }

        public async Task<WebApiAgentResult<ContactCollection>> GetCollAsync(WebApiRequestOptions? requestOptions = null)
        {
            return await base.GetAsync<ContactCollection>("api/v1/contacts/", requestOptions: requestOptions,
                args: new WebApiArg[0]);
        }
    }
}

#nullable restore