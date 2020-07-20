using Beef.WebApi;
using Demo.Service.Common.ServiceAgents;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Generic;
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
                whb.ConfigureLogging(lb =>
                {
                    lb.ClearProviders();
                    lb.AddNUnit();
                    //lb.AddConsole(c => c.DisableColors = true);
                    //lb.AddDebug();
                });

                whb.ConfigureAppConfiguration(cb =>
                {
                    cb.AddInMemoryCollection(new Dictionary<string, string> { { "base-setting", "base-value" }, { "default-setting", "default-value" } });
                });

                whb.ConfigureTestServices(sc =>
                {
                    // TODO - Add default test services, e.g. reference data services, event publishers, etc.
                });

                configuration?.Invoke(whb);
            };            

            _waFactory = _waFactory.WithWebHostBuilder(rootConfiguration);
        }

        private readonly WebApplicationFactory<TEntryPoint> _waFactory;

        public AgentTester<TAgent> Create<TAgent>()
            where TAgent : WebApiServiceAgentBase
        {
            var httpClient = _waFactory.CreateClient();
            return new AgentTester<TAgent>(httpClient, new NUnitTestContextLogger("AgentTester"));
        }
    }

    public class AgentTester<TAgent>
        where TAgent : WebApiServiceAgentBase
    {
        public AgentTester(HttpClient httpClient, ILogger logger)
        {
            _logger = logger;

            var sc = new ServiceCollection();
            sc.AddSingleton<TAgent>();
            sc.Configure<ServiceAgentOptions>(typeof(TAgent).FullName, options =>
            {
                options.HttpClient = httpClient;
            });
            _serviceProvider = sc.BuildServiceProvider();
        }

        private ILogger _logger;
        private IServiceProvider _serviceProvider;
        private HttpStatusCode? _expectedStatusCode;

        public TAgent Agent => _serviceProvider.GetService<TAgent>();

        public AgentTester<TAgent> ExpectStatusCode(HttpStatusCode statusCode)
        {
            _expectedStatusCode = statusCode;
            return this;
        }

        public WebApiAgentResult<TResult> Run<TResult>(Func<TAgent, Task<WebApiAgentResult<TResult>>> invoke)
        {
            var task = invoke(Agent);
            task.Wait();

            LogResult(task.Result);

            ResultCheck(task.Result);

            return task.Result;
        }

        protected void ResultCheck(WebApiAgentResult result)
        {
            if (_expectedStatusCode != null)
                Assert.AreEqual(_expectedStatusCode, result.StatusCode);
        }

        private void LogResult(WebApiAgentResult result)
        {
            _logger.LogInformation($"REQUEST{Environment.NewLine}{result.Request}");
            _logger.LogInformation($"RESPONSE{Environment.NewLine}{result.Response}");
        }
    }
}
