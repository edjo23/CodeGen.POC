#nullable enable

using System;
using System.Net.Http;

namespace Demo.Service.Common.ServiceAgents
{
    public class ServiceAgentOptions
    {
        public HttpClient? HttpClient { get; set; }
        public Action<HttpRequestMessage>? BeforeRequest { get; set; }
    }
}

#nullable restore