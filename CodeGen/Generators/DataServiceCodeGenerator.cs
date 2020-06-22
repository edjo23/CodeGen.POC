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
    public class DataServiceCodeGenerateOptions : CodeGenerateOptions { }

    public class DataServiceCodeGenerator : IGenerator
    {
        public DataServiceCodeGenerator(DataServiceCodeGenerateOptions options, ITemplateProvider templateProvider, ICodeGenerationModelProvider domainDataProvider, IRenderer renderer, IOutputService outputService)
        {
            Options = options;
            TemplateProvider = templateProvider;
            DomainDataProvider = domainDataProvider;
            Renderer = renderer;
            OutputService = outputService;
        }

        public DataServiceCodeGenerateOptions Options { get; set; }
        public ITemplateProvider TemplateProvider { get; set; }
        public ICodeGenerationModelProvider DomainDataProvider { get; set; }
        public IRenderer Renderer { get; set; }
        public IOutputService OutputService { get; set; }

        public void Generate()
        {
            var data = DomainDataProvider.GetData();

            foreach (var entity in data.Entities.Where(o => o.DataServiceClass != null))
            {
                var output = Renderer.Render(entity, TemplateProvider.GetTemplate(Options.TemplatePath));
                var directoryInfo = new DirectoryInfo(Options.TargetPath);
                OutputService.Write(Path.Join(directoryInfo.FullName, $"{entity.DataServiceClass.Name}.cs"), output);
            }
        }
    }
}
