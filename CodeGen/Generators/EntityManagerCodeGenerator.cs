using CodeGen.Artifacts;
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
    public class EntityManagerCodeGenerateOptions : CodeGenerateOptions { }

    class EntityManagerCodeGenerator : IGenerator
    {
        public EntityManagerCodeGenerator(EntityManagerCodeGenerateOptions options, ITemplateProvider templateProvider, ICodeGenerationModelProvider domainDataProvider, IRenderer renderer, IOutputService outputService)
        {
            Options = options;
            TemplateProvider = templateProvider;
            DomainDataProvider = domainDataProvider;
            Renderer = renderer;
            OutputService = outputService;
        }

        public EntityManagerCodeGenerateOptions Options { get; set; }
        public ITemplateProvider TemplateProvider { get; set; }
        public ICodeGenerationModelProvider DomainDataProvider { get; set; }
        public IRenderer Renderer { get; set; }
        public IOutputService OutputService { get; set; }

        public void Generate()
        {
            var data = DomainDataProvider.GetData();

            foreach (var entity in data.Entities.Where(o => o.Manager != null))
            {
                var code = Renderer.Render(entity, TemplateProvider.GetTemplate(Options.TemplatePath));
                OutputService.Write(Path.Join(Options.TargetPath, $"{entity.Manager.Name}.cs"), code);
            }
        }
    }
}
