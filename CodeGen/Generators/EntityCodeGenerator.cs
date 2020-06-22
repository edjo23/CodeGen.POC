using CodeGen.Data;
using CodeGen.Models;
using CodeGen.Generators;
using CodeGen.Renderers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CodeGen.Artifacts
{
    public class EntityCodeGenerateOptions : CodeGenerateOptions { }

    public class EntityCodeGenerator : IGenerator
    {
        public EntityCodeGenerator(EntityCodeGenerateOptions options, ITemplateProvider templateProvider, ICodeGenerationModelProvider domainDataProvider, IRenderer renderer, IOutputService outputService)
        {
            Options = options;
            Template = templateProvider;
            DomainDataProvider = domainDataProvider;
            Renderer = renderer;
            OutputService = outputService;
        }

        public EntityCodeGenerateOptions Options { get; set; }
        public ITemplateProvider Template { get; set; }
        public ICodeGenerationModelProvider DomainDataProvider { get; set; }
        public IRenderer Renderer { get; set; }
        public IOutputService OutputService { get; set; }

        public void Generate()
        {
            var data = DomainDataProvider.GetData();

            foreach (var entity in data.Entities)
            {
                var output = Renderer.Render(entity, Template.GetTemplate(Options.TemplatePath));
                var directoryInfo = new DirectoryInfo(Options.TargetPath);
                OutputService.Write(Path.Join(directoryInfo.FullName, $"{entity.EntityClass.Name}.cs"), output);
            }
        }
    }
}
