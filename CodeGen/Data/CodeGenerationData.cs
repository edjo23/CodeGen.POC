using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGen.Data
{
    public class CodeGenerationData
    {
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
        public bool Abstract { get; set; }
        public string Usings { get; set; }
        public string Implements { get; set; }

        public bool Collection { get; set; }
        public bool CollectionResult { get; set; }
        public string EntityScope { get; set; } = "Common";
        public bool ExcludeEntity { get; set; }
        public bool PartialEntity { get; set; }
        public bool ExcludeIData { get; set; }
        public bool ExcludeData { get; set; }
        public bool PartialData { get; set; }
        public bool PrivateDataConstructor { get; set; }
        public bool ExcludeIDataSvc { get; set; }
        public bool ExcludeDataSvc { get; set; }
        public bool PartialDataSvc { get; set; }
        public bool PrivateDataSvcConstructor { get; set; }
        public bool ExcludeIManager { get; set; }
        public bool ExcludeManager { get; set; }
        public bool PartialManager { get; set; }
        public bool PrivateManagerConstructor { get; set; }
        public bool ExcludeWebApi { get; set; }
        public bool PartialWebApi { get; set; }
        public bool PrivateWebApiConstructor { get; set; }
        public bool ExcludeAll { get; set; }
        public bool DataSvcCaching { get; set; } = true;
        public bool? EventPublish { get; set; } = true;
        public string Validator { get; set; }
        public bool OmitEntityBase { get; set; }
        public bool AutoInferImplements { get; set; } = true;
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
