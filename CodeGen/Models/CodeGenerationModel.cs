using System.Collections.Generic;
using System.Linq;

namespace CodeGen.Models
{
    public class CodeGenerationModel
    {
        public string BaseNamespace { get; set; }
        public IList<Entity> Entities { get; set; }
    }

    public class Entity
    {
        public string Name { get; set; }
        public EntityClass EntityClass { get; set; }
        public DataInterface DataInterface { get; set; }
        public DataClass DataClass { get; set; }
        public DataServiceInterface DataServiceInterface { get; set; }
        public DataServiceClass DataServiceClass { get; set; }
        public ManagerInterface ManagerInterface { get; set; }
        public ManagerClass Manager { get; set; }
        public ControllerClass ControllerClass { get; set; }
    }

    public class EntityClass : ClassData
    {
        public bool Abstract { get; set; }
        public ISet<string> Implements { get; set; } = new HashSet<string>();
        public IList<Property> Properties { get; set; }
        public bool NewtonsoftJsonSerialization { get; set; }
        public string CollectionName { get; set; }
        public ISet<string> CollectionImplements { get; set; } = new HashSet<string>();
        public string CollectionResultName { get; set; }
        public ISet<string> CollectionResultImplements { get; set; } = new HashSet<string>();
        public string Validator { get; set; }
        public bool HasBeefBaseClass { get; set; }
        public bool CollectionHasBeefBaseClass { get; set; }

        public IList<Property> UniqueKeys => Properties.Where(o => o.UnqiueKey).ToList();
        public IList<Property> EntityProperties => Properties.Where(o => o.IsEntity).ToList();
        public IList<Property> DeclareProperties => Properties.Where(o => !o.Inherited).ToList();
        public IList<Property> DeclareUniqueKeys => Properties.Where(o => o.UnqiueKey && !o.Inherited).ToList();
        public IList<Property> DeclareEntityProperties => Properties.Where(o => o.IsEntity && !o.Inherited).ToList();
        public IList<Property> CleanProperties => Properties.Where(o => o.Clean).ToList();
        public bool CollectionKeyed => CollectionImplements.Any(o => o.StartsWith("EntityBaseKeyedCollection"));
    }

    public class Property
    {
        public string Name { get; set; }
        public bool Inherited { get; set; }
        public bool IgnoreSerialization { get; set; }
        public bool EmitDefaultValue { get; set; }
        public string Default { get; set; }
        public string Text { get; set; }
        public string Type { get; set; }
        public bool IsEntity { get; set; }
        public bool BubblePropertyChanged { get; set; }
        public bool Nullable { get; set; }
        public bool Clean { get; set; } = true;
        public bool UnqiueKey { get; set; }
        public bool Immutable { get; set; }
        public string DisplayName { get; set; }
        public string StringTrim { get; set; }
        public string StringTransform { get; set; }
        public string DateTimeTransform { get; set; }
        public bool IsString => Type.ToLower() == "string";
        public bool IsDateTime => Type == "DateTime";
    }

    public class DataInterface : ClassData
    {
    }

    public class DataClass : ClassData
    {
        public List<OperationModelBase> Operations { get; set; } = new List<OperationModelBase>();
    }

    public class DataServiceInterface : ClassData
    {
    }

    public class DataServiceClass : ClassData
    {
        public List<DataServiceOperationModel> Operations { get; set; } = new List<DataServiceOperationModel>();
        public bool HasCaching { get; set; }
    }

    public class DataServiceOperationModel : OperationModelBase
    {
        public string EventSubject { get; set; }
        public string EventAction { get; set; }
    }

    public class ManagerInterface : ClassData
    {
    }

    public class ManagerClass : ClassData
    {
        public List<OperationModelBase> Operations { get; set; } = new List<OperationModelBase>();
    }

    public class ControllerClass : ClassData
    {
        public List<ControllerOperationModel> Operations { get; set; } = new List<ControllerOperationModel>();
        public string WebApiRoutePrefix { get; set; }
    }

    public class ControllerOperationModel : OperationModelBase
    {
        public string WebApiRoute { get; set; }
        public bool HasPagingArgs { get; set; }
    }

    public class ClassData
    {
        public string Name { get; set; }
        public string Namespace { get; set; }
        public ISet<string> Usings { get; set; } = new SortedSet<string>();
        public bool Partial { get; set; }
        public bool PrivateConstructor { get; set; }
    }

    public class OperationParameter
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string EntityProperty { get; set; }
        public bool Mandatory { get; set; }
        public string Validator { get; set; } // TODO Probably should only be a Manager Operation version of class.
    }


    public class OperationModelBase
    {
        public string Name { get; set; }
        public string ReturnType { get; set; }
        public List<OperationParameter> Parameters { get; set; } = new List<OperationParameter>();
        public bool IsCreate { get; set; }
        public bool IsGet { get; set; }
        public bool IsGetColl { get; set; }
        public bool IsUpdate { get; set; }
        public bool IsDelete { get; set; }

        public string UnderlyingReturnType => ReturnType?.TrimEnd('?');
        public OperationParameter Parameter => Parameters.FirstOrDefault(); // TODO Remove
    }
}
