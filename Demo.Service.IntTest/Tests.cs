using Demo.Service.Business;
using Demo.Service.Common.Entities;
using Demo.Service.Common.ServiceAgents;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using NUnit.Framework;
using System;
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
            var result = _defaultFactory.CreateAgentTester<ContactServiceAgent>()
                .ExpectStatusCode(System.Net.HttpStatusCode.OK)
                .Run(o => o.GetCollAsync());

            Assert.IsTrue(result.IsSuccess);
            Assert.AreEqual(1, result.Value.Count);
            Assert.AreEqual(_testId, result.Value[0].Id);
        }


        [Test]
        public void WithCustomFactory()
        {
            var result = _customFactory.CreateAgentTester<ContactServiceAgent>()
                .ExpectStatusCode(System.Net.HttpStatusCode.OK)
                .Run(o => o.GetCollAsync());

            Assert.IsTrue(result.IsSuccess);
        }
    }
}