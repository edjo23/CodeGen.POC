using CodeGen.Data;
using CodeGen.Models;
using CodeGen.Renderers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CodeGen.Generators
{
    public class ServiceCollectionExtensionCodeGenerateOptions : CodeGenerateOptions { }

    public class ServiceCollectionExtensionCodeGenerator : IGenerator
    {
        public ServiceCollectionExtensionCodeGenerator(ServiceCollectionExtensionCodeGenerateOptions options, ITemplateProvider templateProvider, ServiceCollectionExtensionsModelProvider dataProvider, IRenderer renderer, IOutputService outputService)
        {
            Options = options;
            TemplateProvider = templateProvider;
            DataProvider = dataProvider;
            Renderer = renderer;
            OutputService = outputService;
        }

        public ServiceCollectionExtensionCodeGenerateOptions Options { get; set; }
        public ITemplateProvider TemplateProvider { get; set; }
        public ServiceCollectionExtensionsModelProvider DataProvider { get; set; }
        public IRenderer Renderer { get; set; }
        public IOutputService OutputService { get; set; }

        public void Generate()
        {
            var data = DataProvider.GetData();

            var output = Renderer.Render(data, TemplateProvider.GetTemplate(Options.TemplatePath));
            var directoryInfo = new DirectoryInfo(Options.TargetPath);
            OutputService.Write(Path.Join(directoryInfo.FullName, $"ServiceCollectionExtensions.cs"), output);
        }
    }
}
