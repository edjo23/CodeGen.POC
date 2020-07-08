using Demo.Service.Business;
using Demo.Service.Business.DataSvc;
using Demo.Service.Common.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Demo.Service.IntTest
{
    public class VanillaTests
    {
        private WebApplicationFactory<Startup> _factory;

        [OneTimeSetUp]
        public void Setup()
        {
            _factory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(whb =>
                {
                    whb.ConfigureTestServices(sc =>
                    {

                    });
                });
        }

        [Test]
        public async Task VanillaTest()
        {
            var client = _factory.CreateClient();
            var result = await client.GetAsync("/api/v1/contacts");

            Assert.IsTrue(result.IsSuccessStatusCode);
        }


        [Test]
        public async Task VanillaTestWithWebHostBuilder()
        {
            var mockService = new Mock<IContactManager>();
            mockService.Setup(o => o.GetCollAsync()).Returns(Task.FromResult(new ContactCollection() { new Contact { Id = Guid.Empty } }));

            var client = _factory.WithWebHostBuilder(whb =>
            {
                whb.ConfigureTestServices(sc =>
                {
                    sc.Replace(ServiceDescriptor.Transient<IContactManager>((sp) => mockService.Object));
                });
            }).CreateClient(); ;
            var result = await client.GetAsync("/api/v1/contacts");

            Assert.IsTrue(result.IsSuccessStatusCode);
        }
    }
}