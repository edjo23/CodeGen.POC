using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGen.Models
{
    public class ServiceCollectionExtensionsModel
    {
        public string Namespace { get; set; }
        public IList<string> Usings { get; set; } = new List<string>();
        public IList<Entity> EntitiesWithDataClasses { get; set; }
        public IList<Entity> EntitiesWithDataServiceClasses { get; set; }
        public IList<Entity> EntitiesWithManagerClasses { get; set; }
    }
}
