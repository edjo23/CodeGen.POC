#nullable enable

using Beef.WebApi;
using Demo.Service.Common.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Demo.Service.Common.ServiceAgents
{
    public partial class PersonServiceAgent
    {
        public async Task<WebApiAgentResult<string>> GetConfig(string key)
        {
            return await base.GetAsync<string>("api/v1/persons/config/{key}", requestOptions: null,
                args: new WebApiArg[] { new WebApiArg<string>("key", key) });
        }
    }
}

#nullable restore