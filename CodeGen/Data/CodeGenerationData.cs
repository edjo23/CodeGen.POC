using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGen.Data
{
    public class CodeGenerationData
    {
        public string EntityScope { get; set; } = "Common";
        public string BaseNamespace { get; set; }

        public string RefDataNamespace { get; set; }
        public IList<Entity> Entities { get; set; }
        public bool? EventPublish { get; set; } = true;
    }

    public class Entity
    {
        public string Name { get; set; }
        public string WebApiRoutePrefix { get; set; }
        public IList<Property> Properties { get; set; }
        public IList<Operations> Operations { get; set; }
        public bool Collection { get; set; }
        public bool CollectionResult { get; set; }
        public bool PartialController { get; set; }
        public bool PrivateControllerConstructor { get; set; }
        public bool PartialManager { get; set; }
        public bool PrivateManagerConstructor { get; set; }
        public bool PartialDataSvc { get; set; }
        public bool PartialData { get; set; }
        public bool DataSvcCaching { get; set; } = true;
        public bool? EventPublish { get; set; } = true;
        public string Validator { get; set; }
    }

    public class Property
    {
        public string Name { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public bool UniqueKey { get; set; }
        public bool IsEntity { get; set; }
        public bool BubblePropertyChanged { get; set; }
    }

    public class Operations
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string WebApiRoute { get; set; }
        public bool PagingArgs { get; set; }
        public IList<OperationParameter> Parameters { get; set; }
        public bool? EventPublish { get; set; }
        public string Validator { get; set; }
    }

    public class OperationParameter
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool? Mandatory { get; set; }
        public string Validator { get; set; }
    }
}
