﻿using CodeGen.Models;
using CodeGen.Renderers;
using System.IO;
using System.Linq;

namespace CodeGen.Generators
{
    public class EntityWebApiControllerCodeGenerateOptions : CodeGenerateOptions { }

    public class EntityWebApiControllerCodeGenerator : IGenerator
    {
        public EntityWebApiControllerCodeGenerator(EntityWebApiControllerCodeGenerateOptions options, ITemplateProvider templateProvider, ICodeGenerationModelProvider domainDataProvider, IRenderer renderer, IOutputService outputService)
        {
            Options = options;
            TemplateProvider = templateProvider;
            DomainDataProvider = domainDataProvider;
            Renderer = renderer;
            OutputService = outputService;
        }

        public EntityWebApiControllerCodeGenerateOptions Options { get; set; }
        public ITemplateProvider TemplateProvider { get; set; }
        public ICodeGenerationModelProvider DomainDataProvider { get; set; }
        public IRenderer Renderer { get; set; }
        public IOutputService OutputService { get; set; }

        public void Generate()
        {
            var data = DomainDataProvider.GetData();

            foreach (var entity in data.Entities.Where(o => o.ControllerClass != null))
            {
                var output = Renderer.Render(entity, TemplateProvider.GetTemplate(Options.TemplatePath));
                var directoryInfo = new DirectoryInfo(Options.TargetPath);
                OutputService.Write(Path.Join(directoryInfo.FullName, $"{entity.ControllerClass.Name}.cs"), output);
            }
        }
    }
}
