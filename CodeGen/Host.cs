using CodeGen.Artifacts;
using CodeGen.Data;
using CodeGen.Generators;
using CodeGen.Models;
using CodeGen.Renderers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CodeGen
{
    public class HostBuilder
    {
        public static HostBuilder Create()
        {
            return new HostBuilder();
        }

        private Type _startupType;

        public HostBuilder WithStartup<T>()
            where T : class, new()
        {
            if (_startupType != null)
                throw new InvalidOperationException("WithStartup<T>() can only be called once");

            _startupType = typeof(T);

            return this;
        }

        public Host Build()
        {
            var serviceCollection = ConfigureServiceCollection();
            object startup = null;

            if (_startupType != null)
            {
                var startupServiceCollection = new ServiceCollection();
                startupServiceCollection.AddSingleton(_startupType);
                startupServiceCollection.AddSingleton<IServiceCollection>(sp => serviceCollection);
                
                var startupServiceProvider = startupServiceCollection.BuildServiceProvider();
                startup = startupServiceProvider.GetService(_startupType);

                InvokeStartupMethod("ConfigureServices", startup, startupServiceProvider);
            }

            serviceCollection
                .AddSingleton<IOutputService, OutputService>()
                .AddSingleton<Host>()
                .AddGenerators();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            if (startup != null)
            {
                InvokeStartupMethod("Configure", startup, serviceProvider);
            }

            return serviceProvider.GetService<Host>();
        }

        private void InvokeStartupMethod(string methodName, object startup, IServiceProvider startupServiceProvider)
        {
            var method = startup.GetType().GetMethod(methodName);

            if (method == null || method.GetParameters().Any(o => o.IsOut))
                return;

            var paramValues = method.GetParameters().Select(o => startupServiceProvider.GetService(o.ParameterType)).ToArray();

            method.Invoke(startup, paramValues);
        }

        private ServiceCollection ConfigureServiceCollection()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<IConfiguration>(sp =>
            {
                var cb = new ConfigurationBuilder()
                    .AddInMemoryCollection(new Dictionary<string, string>
                    {
                        { "Company", "Test" },
                        { "AppName", "TestService" },

                        { "Generator:Entity:TemplatePath", "Templates/Entity_cs.hb" },
                        { "Generator:Entity:TargetPath", "../../../../$()/Entities/Generated" },

                        { "Generator:IData:TemplatePath", "Templates/EntityDataInterface_cs.hb" },
                        { "Generator:IData:TargetPath", "../../../../$()/Business/Data/Generated" },
                        { "Generator:Data:TemplatePath", "Templates/EntityData_cs.hb" },
                        { "Generator:Data:TargetPath", "../../../../$()/Business/Data/Generated" },

                        { "Generator:IDataService:TemplatePath", "Templates/EntityDataSvcInterface_cs.hb" },
                        { "Generator:IDataService:TargetPath", "../../../../$()/Business/DataSvc/Generated" },
                        { "Generator:DataService:TemplatePath", "Templates/EntityDataSvc_cs.hb" },
                        { "Generator:DataService:TargetPath", "../../../../$()/Business/DataSvc/Generated" },

                        { "Generator:IEntityManager:TemplatePath", "Templates/EntityManagerInterface_cs.hb" },
                        { "Generator:IEntityManager:TargetPath", "../../../../$()/Business/Generated" },
                        { "Generator:EntityManager:TemplatePath", "Templates/EntityManager_cs.hb" },
                        { "Generator:EntityManager:TargetPath", "../../../../$()/Business/Generated" },

                        { "Generator:Controller:TemplatePath", "Templates/EntityWebApiController_cs.hb" },
                        { "Generator:Controller:TargetPath", "../../../../$()/Controllers/Generated" },
                        { "Generator:ServiceAgent:TemplatePath", "Templates/EntityWebApiServiceAgent_cs.hb" },
                        { "Generator:ServiceAgent:TargetPath", "../../../../$()/Common/ServiceAgents/Generated" },

                        { "Generator:ServiceCollectionExtension:TemplatePath", "Templates/ServiceCollectionExtension_cs.hb" },
                        { "Generator:ServiceCollectionExtension:TargetPath", "../../../../$()/Generated" },
                    })
                    .AddJsonFile("appsettings.json");
                return cb.Build();
            });

            serviceCollection.AddOption((sp, c) => c.BindConfig("CodeGenerationData", new CodeGenerationDataOptions()));

            serviceCollection.AddSingleton<ICodeGenerationDataProvider, CodeGenerationDataProvider>();
            serviceCollection.AddSingleton<ICodeGenerationModelProvider, CodeGenerationModelProvider>();
            serviceCollection.AddTransient<ITemplateProvider, HandlebarsTemplateProvider>();
            serviceCollection.AddTransient<IRenderer, HandlebarsRenderer>();

            serviceCollection.AddOption((sp, c) => c.BindConfig("Generator:Entity", new EntityCodeGenerateOptions()));
            serviceCollection.AddTransient<EntityCodeGenerator>();

            serviceCollection.AddOption((sp, c) => c.BindConfig("Generator:IData", new EntityDataInterfaceCodeGenerateOptions()));
            serviceCollection.AddTransient<EntityDataInterfaceCodeGenerator>();

            serviceCollection.AddOption((sp, c) => c.BindConfig("Generator:Data", new EntityDataCodeGenerateOptions()));
            serviceCollection.AddTransient<EntityDataCodeGenerator>();

            serviceCollection.AddOption((sp, c) => c.BindConfig("Generator:IDataService", new EntityDataServiceInterfaceCodeGenerateOptions()));
            serviceCollection.AddTransient<EntityDataServiceInterfaceCodeGenerator>();

            serviceCollection.AddOption((sp, c) => c.BindConfig("Generator:DataService", new EntityDataServiceCodeGenerateOptions()));
            serviceCollection.AddTransient<EntityDataServiceCodeGenerator>();

            serviceCollection.AddOption((sp, c) => c.BindConfig("Generator:IEntityManager", new EntityManagerInterfaceCodeGenerateOptions()));
            serviceCollection.AddTransient<EntityManagerInterfaceCodeGenerator>();

            serviceCollection.AddOption((sp, c) => c.BindConfig("Generator:EntityManager", new EntityManagerCodeGenerateOptions()));
            serviceCollection.AddTransient<EntityManagerCodeGenerator>();

            serviceCollection.AddOption((sp, c) => c.BindConfig("Generator:Controller", new EntityWebApiControllerCodeGenerateOptions()));
            serviceCollection.AddTransient<EntityWebApiControllerCodeGenerator>();

            serviceCollection.AddOption((sp, c) => c.BindConfig("Generator:ServiceAgent", new EntityWebApiServiceAgentCodeGenerateOptions()));
            serviceCollection.AddTransient<EntityWebApiServiceAgentCodeGenerator>();

            serviceCollection.AddOption((sp, c) => c.BindConfig("Generator:ServiceCollectionExtension", new ServiceCollectionExtensionCodeGenerateOptions()));
            serviceCollection.AddSingleton<ServiceCollectionExtensionsModelProvider>();
            serviceCollection.AddTransient<ServiceCollectionExtensionCodeGenerator>();

            return serviceCollection;
        }
    }

    public class Host
    {
        public Host(IServiceProvider serviceProvider, IEnumerable<IGenerator> generators, IOutputService outputService)
        {
            Services = serviceProvider;
            _generators = generators;
            _outputService = outputService;
        }

        private readonly IEnumerable<IGenerator> _generators;
        private readonly IOutputService _outputService;

        public IServiceProvider Services { get; private set; }

        public int Run()
        {
            foreach (var generator in _generators)
                generator.Generate();

            return _outputService.UpdatedCount;
        }
    }
}
