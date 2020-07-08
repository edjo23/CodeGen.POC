using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework.Internal;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Demo.Service.IntTest
{    
    public class TestFactory<TEntryPoint> : TestFactory<WebApplicationFactory<TEntryPoint>, TEntryPoint>
        where TEntryPoint : class
    {
        public TestFactory(Action<IWebHostBuilder> configuration = null) : base (configuration)
        {
        }
    }

    public class TestFactory<TFactory, TEntryPoint>
        where TFactory : WebApplicationFactory<TEntryPoint>, new()
        where TEntryPoint : class
    {
        public TestFactory(Action<IWebHostBuilder> configuration = null)
        {
            _waFactory = new TFactory();

            if (configuration != null)
                _waFactory = _waFactory.WithWebHostBuilder(configuration);
        }

        private readonly WebApplicationFactory<TEntryPoint> _waFactory;

        public T CreateAgent<T>()
            where T : class
        {
            var httpClient = _waFactory.CreateClient();

            return Activator.CreateInstance(typeof(T), new object[] { httpClient, null }) as T;
        }
    }
}
