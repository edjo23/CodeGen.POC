using System.IO;

namespace CodeGen.Renderers

{
    public interface ITemplateProvider
    {
        string GetTemplate(string path);
    }

    public class HandlebarsTemplateProvider : ITemplateProvider
    {
        public string GetTemplate(string path)
        {
            return File.ReadAllText(path);
        }
    }
}
