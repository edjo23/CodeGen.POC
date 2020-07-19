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
using System.Threading;
using System.Threading.Tasks;

//[assembly: LevelOfParallelism(3)]

namespace Demo.Service.IntTest
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class ParallelTests
    {
        private AgentTesterFactory<Startup> _defaultFactory;

        [OneTimeSetUp]
        public void Setup()
        {
            _defaultFactory = new AgentTesterFactory<Startup>();

            // Reduces queuing (but doesn't eliminate it) for the first batch of parallel tests.
            _defaultFactory.Create<ContactServiceAgent>()
                .Run(o => o.GetCollAsync());
                //.ExpectStatusCode(System.Net.HttpStatusCode.NotFound)
                //.Run(o => o.GetAsync<object>("notfound"));
        }

        [Test]
        public void Test1()
        {
            var result = _defaultFactory.Create<ContactServiceAgent>()
                .ExpectStatusCode(System.Net.HttpStatusCode.OK)
                .Run(o => o.GetCollAsync());

            Assert.IsTrue(result.IsSuccess);
        }

        [Test]
        public void Test2() => Test1();

        [Test]
        public void Test3() => Test1();

        [Test]
        public void Test4() => Test1();


        [Test]
        public void Test5() => Test1();

        [Test]
        public void Test6() => Test1();

        [Test]
        public void Test7() => Test1();

        [Test]
        public void Test8() => Test1();

        [Test]
        public void Test9() => Test1();

        [Test]
        public void Test10() => Test1();

        [Test]
        public void Test11() => Test1();

        [Test]
        public void Test12() => Test1();
    }
}