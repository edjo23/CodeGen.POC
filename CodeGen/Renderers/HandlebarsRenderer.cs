using CodeGen.Data;
using CodeGen.Models;
using HandlebarsDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace CodeGen.Renderers
{
    public interface IRenderer
    {
        string Render(object data, string source);
    }

    public class HandlebarsRenderer : IRenderer
    {
        static HandlebarsRenderer()
        {
            Handlebars.RegisterHelper("join", (writer, context, parameters) => {
                var items = parameters.FirstOrDefault();
                var text = items != null && items is IEnumerable<string> stringItems ? String.Join(", ", stringItems) : "";
                writer.WriteSafeString(text);
            });

            Handlebars.RegisterHelper("camel", (writer, context, parameters) => {
                var items = parameters.FirstOrDefault();
                var text = items != null ? items.ToString().ToCamelCase() : "";
                writer.WriteSafeString(text);
            });
        }

        public string Render(object data, string source)
        {
            var template = Handlebars.Compile(source);
            var output = template(data);
            return output;
        }        
    }
}
