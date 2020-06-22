using CodeGen;
using System;

namespace Demo.CodeGen
{
    class Program
    {
        static int Main(string[] args)
        {
            var host = HostBuilder
                .Create()
                .WithStartup<Startup>()
                .Build();

            return host.Run();
        }
    }
}