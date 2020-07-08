#nullable enable

using Beef.WebApi;
using Demo.Service.Common.Entities;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Demo.Service.Common.ServiceAgents
{
    public class ContactServiceAgent : WebApiServiceAgentBase<ContactServiceAgent>
    {
        public ContactServiceAgent() : base() { }
        public ContactServiceAgent(HttpClient? httpClient = null, Action<HttpRequestMessage>? beforeRequest = null) : base(httpClient, beforeRequest) { }

        public async Task<WebApiAgentResult<ContactCollection>> GetCollAsync()
        {
            return await base.GetAsync<ContactCollection>("api/v1/contacts/", requestOptions: null,
                args: Array.Empty<WebApiArg>());
        }
    }
}

#nullable restore