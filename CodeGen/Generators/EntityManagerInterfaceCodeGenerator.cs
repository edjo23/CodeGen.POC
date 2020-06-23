using CodeGen.Models;
using CodeGen.Renderers;
using System.IO;
using System.Linq;

namespace CodeGen.Generators
{
    public class EntityManagerInterfaceCodeGenerateOptions : CodeGenerateOptions { }

    public class EntityManagerInterfaceCodeGenerator : IGenerator
    {
        public EntityManagerInterfaceCodeGenerator(EntityManagerInterfaceCodeGenerateOptions options, ITemplateProvider templateProvider, ICodeGenerationModelProvider domainDataProvider, IRenderer renderer, IOutputService outputService)
        {
            Options = options;
            TemplateProvider = templateProvider;
            DomainDataProvider = domainDataProvider;
            Renderer = renderer;
            OutputService = outputService;
        }

        public EntityManagerInterfaceCodeGenerateOptions Options { get; set; }
        public ITemplateProvider TemplateProvider { get; set; }
        public ICodeGenerationModelProvider DomainDataProvider { get; set; }
        public IRenderer Renderer { get; set; }
        public IOutputService OutputService { get; set; }

        public void Generate()
        {
            var data = DomainDataProvider.GetData();

            foreach (var entity in data.Entities.Where(o => o.ManagerInterface != null))
            {
                var code = Renderer.Render(entity, TemplateProvider.GetTemplate(Options.TemplatePath));
                OutputService.Write(Path.Join(Options.TargetPath, $"{entity.ManagerInterface.Name}.cs"), code);
            }
        }
    }
}
