#nullable enable

using Beef.WebApi;
using Demo.Service.Common.Entities;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Demo.Service.Common.ServiceAgents
{
    public class PersonServiceAgent : WebApiServiceAgentBase<PersonServiceAgent>
    {
        public PersonServiceAgent() : base() { }
        public PersonServiceAgent(HttpClient? httpClient = null, Action<HttpRequestMessage>? beforeRequest = null) : base(httpClient, beforeRequest) { }

    }
}

#nullable restore