using CodeGen.Artifacts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CodeGen.Generators
{
    public interface ITemplateProvider
    {
        string GetTemplate(string path);
    }

    public class TemplateProvider
    {
        public TemplateProvider(CodeGenerateOptions options)
        {
            Options = options;
        }

        public CodeGenerateOptions Options { get; }

        public string GetTemplate()
        {
            return File.ReadAllText(Options.TemplatePath);
        }

    }

    public class HandlebarsTemplateProvider : ITemplateProvider
    {
        public string GetTemplate(string path)
        {
            return File.ReadAllText(path);
        }
    }
}
