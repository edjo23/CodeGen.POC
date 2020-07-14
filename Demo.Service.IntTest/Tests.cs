using Demo.Service.Business;
using Demo.Service.Common.Entities;
using Demo.Service.Common.ServiceAgents;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Service.IntTest
{
    public class Tests
    {
        private AgentTesterFactory<Startup> _defaultFactory;
        private AgentTesterFactory<CustomWebApplicationFactory, Startup> _customFactory;
        private Guid _testId = Guid.NewGuid();

        [OneTimeSetUp]
        public void Setup()
        {
            var mockService = new Mock<IContactManager>();
            mockService.Setup(o => o.GetCollAsync()).Returns(Task.FromResult(new ContactCollection() { new Contact { Id = _testId } }));

            _defaultFactory = new AgentTesterFactory<Startup>(whb =>
            {
                whb.ConfigureAppConfiguration(cb =>
                {
                    cb.AddInMemoryCollection(new Dictionary<string, string> { { "default-setting", "custom-value" } });
                });

                whb.ConfigureTestServices(sc =>
                {
                    sc.Replace(ServiceDescriptor.Transient<IContactManager>((sp) => mockService.Object));
                });
            });

            _customFactory = new AgentTesterFactory<CustomWebApplicationFactory, Startup>();
        }

        [Test]
        public void WithDefaultFactory()
        {
            var result = _defaultFactory.Create<ContactServiceAgent>()
                .ExpectStatusCode(System.Net.HttpStatusCode.OK)
                .Run(o => o.GetCollAsync());

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(1, result.Value.Count);
            Assert.AreEqual(_testId, result.Value[0].Id);
        }


        [Test]
        public void WithCustomFactory()
        {
            var result = _customFactory.Create<ContactServiceAgent>()
                .ExpectStatusCode(System.Net.HttpStatusCode.OK)
                .Run(o => o.GetCollAsync());

            Assert.IsTrue(result.IsSuccess);
        }

        [Test]
        public void WithBaseConfig()
        {
            var result = _defaultFactory.Create<PersonServiceAgent>()
                .ExpectStatusCode(System.Net.HttpStatusCode.OK)
                .Run(o => o.GetConfig("base-setting"));

            Assert.AreEqual("base-value", result.Value);
        }

        [Test]
        public void WithFixtureConfig()
        {
            var result = _defaultFactory.Create<PersonServiceAgent>()
                .ExpectStatusCode(System.Net.HttpStatusCode.OK)
                .Run(o => o.GetConfig("default-setting"));

            Assert.AreEqual("custom-value", result.Value);
        }
    }
}