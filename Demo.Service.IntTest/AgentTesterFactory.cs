using Beef.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Demo.Service.IntTest
{
    public class AgentTesterFactory<TEntryPoint> : AgentTesterFactory<WebApplicationFactory<TEntryPoint>, TEntryPoint>
        where TEntryPoint : class
    {
        public AgentTesterFactory(Action<IWebHostBuilder> configuration = null) : base(configuration)
        {
        }
    }

    public class AgentTesterFactory<TFactory, TEntryPoint>
        where TFactory : WebApplicationFactory<TEntryPoint>, new()
        where TEntryPoint : class
    {
        public AgentTesterFactory(Action<IWebHostBuilder> configuration = null)
        {
            _waFactory = new TFactory();

            Action<IWebHostBuilder> rootConfiguration = (whb) =>
            {
                // TODO - Add default test services, e.g. reference data services, event publishers, etc.
                configuration?.Invoke(whb);
            };

            _waFactory = _waFactory.WithWebHostBuilder(rootConfiguration);
        }

        private readonly WebApplicationFactory<TEntryPoint> _waFactory;

        public AgentTester<TAgent> CreateTester<TAgent>()
            where TAgent : class
        {
            var httpClient = _waFactory.CreateClient();
            return new AgentTester<TAgent>(httpClient);
        }
    }

    public class AgentTester<TAgent>
        where TAgent : class
    {
        public AgentTester(HttpClient httpClient)
        {
            Agent = Activator.CreateInstance(typeof(TAgent), new object[] { httpClient, null }) as TAgent;
        }

        public readonly TAgent Agent;
        private HttpStatusCode? _expectedStatusCode;

        public AgentTester<TAgent> ExpectStatusCode(HttpStatusCode statusCode)
        {
            _expectedStatusCode = statusCode;
            return this;
        }

        public WebApiAgentResult<TResult> Run<TResult>(Func<TAgent, Task<WebApiAgentResult<TResult>>> invoke)
        {
            var result = invoke(Agent).Result;

            ResultCheck(result);

            return result;
        }

        protected void ResultCheck(WebApiAgentResult result)
        {
            if (_expectedStatusCode != null)
                Assert.AreEqual(_expectedStatusCode, result.StatusCode);
        }
    }
}
