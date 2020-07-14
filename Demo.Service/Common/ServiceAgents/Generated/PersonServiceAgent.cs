#nullable enable

using Beef.Entities;
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

        public async Task<WebApiAgentResult<Person?>> GetAsync(Guid id, WebApiRequestOptions? requestOptions = null)
        {
            return await base.GetAsync<Person?>("api/v1/persons/{id}", requestOptions: requestOptions,
                args: new WebApiArg[] { new WebApiArg<Guid>("id", id) });
        }

        public async Task<WebApiAgentResult<PersonCollectionResult>> GetByArgsAsync(PersonArgs args, PagingArgs? pagingArgs, WebApiRequestOptions? requestOptions = null)
        {
            return await base.GetAsync<PersonCollectionResult>("api/v1/persons/", requestOptions: requestOptions,
                args: new WebApiArg[] { new WebApiArg<PersonArgs>("args", args), new WebApiArg<PagingArgs?>("pagingArgs", pagingArgs) });
        }
    }
}

#nullable restore