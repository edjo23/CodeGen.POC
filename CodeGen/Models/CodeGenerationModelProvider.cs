#nullable enable

using CodeGen.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace CodeGen.Models
{
    public interface ICodeGenerationModelProvider
    {
        CodeGenerationModel GetData();
    }

    public class CodeGenerationModelOptions
    {
        public string? DataFilePath { get; set; }
    }

    public class CodeGenerationModelProvider : ICodeGenerationModelProvider
    {
        public CodeGenerationModelProvider(CodeGenerationModelOptions options)
        {
            _options = options;
        }

        private readonly CodeGenerationModelOptions _options;
        private CodeGenerationModel? _data;

        public CodeGenerationModel GetData()
        {
            // TODO use mem cache
            if (_data == null)
            {
                var deserializer = new DeserializerBuilder()
                    .WithNamingConvention(CamelCaseNamingConvention.Instance)
                    .IgnoreUnmatchedProperties()
                    .Build();
                var config = deserializer.Deserialize<Data.CodeGenerationData>(File.ReadAllText(_options.DataFilePath ?? "Data.yaml"));

                _data = Transform(config);
            }
            return _data;
        }

        private CodeGenerationModel Transform(CodeGenerationData genConfig)
        {
            var model = new CodeGenerationModel();
            model.BaseNamespace = genConfig.BaseNamespace;
            model.Entities = genConfig.Entities.Select(o => TransformToEntityModel(model, o)).ToList();

            return model;
        }

        private Entity TransformToEntityModel(CodeGenerationModel genModel, Data.Entity entityConfig)
        {
            var model = new Entity
            {
                Name = entityConfig.Name,
                EntityClass = TransformToEntityClass(entityConfig, genModel)
            };

            if (entityConfig.Operations != null && entityConfig.Operations.Count > 0)
            {
                model.DataInterface = TransformToDataInterface(entityConfig, genModel, model.EntityClass);
                model.DataClass = TransformToDataClass(entityConfig, genModel, model.EntityClass);
                model.DataServiceInterface = TransformToDataServiceInterface(entityConfig, genModel, model.EntityClass);
                model.DataServiceClass = TransformToDataServiceClass(entityConfig, genModel, model.EntityClass, model.DataInterface);
                model.ManagerInterface = TransformToManagerInterface(entityConfig, genModel, model.EntityClass);
                model.Manager = TransformToManagerClass(entityConfig, genModel, model.EntityClass, model.DataServiceInterface);
                model.ControllerClass = TransformToControllerClass(entityConfig, genModel, model.EntityClass, model.ManagerInterface);
            }

            return model;
        }

        private EntityClass TransformToEntityClass(Data.Entity entityConfig, CodeGenerationModel genModel)
        {
            var scope = "Common";

            var data = new EntityClass
            {
                Name = entityConfig.Name,
                Namespace = $"{genModel.BaseNamespace}.{scope}.Entities",
                Validator = entityConfig.Validator
            };

            data.Usings.Add($"System");
            data.Usings.Add($"System.ComponentModel.DataAnnotations");
            data.Usings.Add($"Beef.Entities");

            data.Inherits = "EntityBase";
            data.Implements.Add(data.Inherits);

            data.Properties = entityConfig.Properties?.Select(o => TransformToProperty(o)).ToList() ?? new List<Property>();

            // Add IIdentifier
            if (data.Properties.Any(o => o.Name == "Id" && o.Type == "Guid"))
                data.Implements.Add("IGuidIdentifier");

            // Add IETag
            if (data.Properties.Any(o => o.Name == "ETag" && o.Type == "string"))
                data.Implements.Add("IETag");

            // Add ChangeLog
            if (data.Properties.Any(o => o.Name == "ChangeLog" && o.Type == "ChangeLog"))
                data.Implements.Add("IChangeLog");

            // Add IEquatable
            data.Implements.Add($"IEquatable<{data.Name}>");

            // Add Newtonsoft serialisation
            data.NewtonsoftJsonSerialization = true;
            data.Usings.Add("Newtonsoft.Json");

            // Has collection?
            data.CollectionName = entityConfig.Collection ? $"{data.Name}Collection" : null;
            if (data.CollectionName != null)
                data.Usings.Add("System.Collections.Generic");

            // Has collection result?
            data.CollectionResultName = entityConfig.CollectionResult ? $"{data.Name}CollectionResult" : null;

            return data;
        }

        private Property TransformToProperty(Data.Property config)
        {
            var data = new Property();
            data.Name = config.Name;
            data.Type = config.Type;
            data.Text = config.Text ?? config.Name;
            data.IsEntity = config.IsEntity;
            data.BubblePropertyChanged = config.BubblePropertyChanged || config.IsEntity;
            data.Nullable = !config.UniqueKey;
            data.UnqiueKey = config.UniqueKey;
            data.Immutable = false;
            data.DisplayName = data.Name.ToDisplayName();

            if (data.IsString)
            {
                data.StringTrim = "UseDefault";
                data.StringTransform = "UseDefault";
            }

            if (data.IsDateTime)
            {
                data.DateTimeTransform = "UseDefault";
            }

            return data;
        }

        private DataInterface TransformToDataInterface(Data.Entity entityConfig, CodeGenerationModel genModel, EntityClass entityClass)
        {
            var data = new DataInterface
            {
                Name = $"I{entityClass.Name}Data",
                Namespace = $"{genModel.BaseNamespace}.Business.Data"
            };

            data.Usings.Add("System");
            data.Usings.Add("System.Threading.Tasks");
            data.Usings.Add(entityClass.Namespace);

            if (entityConfig.Operations?.Any(o => o.Type == "GetColl" && o.PagingArgs) == true)
                data.Usings.Add("Beef.Entities");

            return data;
        }

        private DataClass TransformToDataClass(Data.Entity entityConfig, CodeGenerationModel genModel, EntityClass entityClass)
        {
            var data = new DataClass
            {
                Name = $"{entityClass.Name}Data",
                Namespace = $"{genModel.BaseNamespace}.Business.Data"
            };

            data.Usings.Add("System");
            data.Usings.Add("System.Threading.Tasks");
            data.Usings.Add(entityClass.Namespace);

            if (entityConfig.Operations != null)
            {
                data.Operations = entityConfig.Operations.Select(o =>
                {
                    var op = CreateOperation<OperationModelBase>(o, entityClass);

                    if (op.IsGetColl && o.PagingArgs)
                    {
                        data.Usings.Add("Beef.Entities");
                        op.Parameters.Add(new OperationParameterModel { Name = "pagingArgs", Type = "PagingArgs?" });
                    }

                    return op;
                }).ToList();
            }

            return data;
        }

        private DataServiceInterface TransformToDataServiceInterface(Data.Entity entityConfig, CodeGenerationModel genModel, EntityClass entityClass)
        {
            var data = new DataServiceInterface
            {
                Name = $"I{entityClass.Name}DataSvc",
                Namespace = $"{genModel.BaseNamespace}.Business.DataSvc"
            };

            data.Usings.Add("System");
            data.Usings.Add("System.Threading.Tasks");
            data.Usings.Add(entityClass.Namespace);

            if (entityConfig.Operations?.Any(o => o.PagingArgs) == true)
                data.Usings.Add("Beef.Entities");

            return data;
        }

        private DataServiceClass TransformToDataServiceClass(Data.Entity entityConfig, CodeGenerationModel genModel, EntityClass entityClass, DataInterface dataInterface)
        {
            var data = new DataServiceClass
            {
                Name = $"{entityClass.Name}DataSvc",
                Namespace = $"{genModel.BaseNamespace}.Business.DataSvc",
                HasCaching = entityConfig.DataSvcCaching
            };

            data.Usings.Add("Beef");
            data.Usings.Add("Beef.Business");
            data.Usings.Add("Beef.Entities");
            data.Usings.Add("System");
            data.Usings.Add("System.Threading.Tasks");
            data.Usings.Add(entityClass.Namespace);
            data.Usings.Add(dataInterface.Namespace);

            if (entityConfig.Operations != null)
            {
                data.Operations = entityConfig.Operations.Select(o =>
                {
                    var op = CreateOperation<DataServiceOperationModel>(o, entityClass);
                    op.EventSubject = entityClass.Name;
                    op.EventAction = o.EventPublish ?? entityConfig.EventPublish ?? true ? (op.IsCreate ? "Created" : op.IsUpdate ? "Updated" : op.IsDelete ? "Deleted" : null) : null;

                    if (op.IsGetColl && o.PagingArgs)
                        op.Parameters.Add(new OperationParameterModel { Name = "pagingArgs", Type = "PagingArgs?" });

                    return op;
                }).ToList();
            }

            return data;
        }

        private ManagerInterface TransformToManagerInterface(Data.Entity entityConfig, CodeGenerationModel genModel, EntityClass entityClass)
        {
            var data = new ManagerInterface
            {
                Name = $"I{entityClass.Name}Manager",
                Namespace = $"{genModel.BaseNamespace}.Business"
            };

            data.Usings.Add("System");
            data.Usings.Add("System.Threading.Tasks");
            data.Usings.Add(entityClass.Namespace);

            if (entityConfig.Operations?.Any(o => o.PagingArgs) == true)
                data.Usings.Add("Beef.Entities");

            return data;
        }

        private ManagerClass TransformToManagerClass(Data.Entity entityConfig, CodeGenerationModel genModel, EntityClass entityClass, DataServiceInterface dataServiceInterface)
        {
            var data = new ManagerClass
            {
                Name = $"{entityClass.Name}Manager",
                Namespace = $"{genModel.BaseNamespace}.Business",
                Partial = entityConfig.PartialManager,
                PrivateConstructor = entityConfig.PrivateManagerConstructor
        };

            data.Usings.Add("Beef");
            data.Usings.Add("Beef.Business");
            data.Usings.Add("Beef.Entities");
            data.Usings.Add("Beef.Validation");
            data.Usings.Add("System");
            data.Usings.Add("System.Threading.Tasks");
            data.Usings.Add(entityClass.Namespace);
            data.Usings.Add(dataServiceInterface.Namespace);

            if (entityConfig.Operations != null)
            {
                data.Operations = entityConfig.Operations.Select(o =>
                {
                    var op = CreateOperation<OperationModelBase>(o, entityClass);

                    if (op.IsGetColl && o.PagingArgs)
                        op.Parameters.Add(new OperationParameterModel { Name = "pagingArgs", Type = "PagingArgs?" });

                    return op;
                }).ToList();
            }

            if (data.Operations.SelectMany(o => o.Parameters ?? new List<OperationParameterModel>()).Any(o => o.Validator != null))
                data.Usings.Add($"{data.Namespace}.Validation");

            return data;
        }

        private ControllerClass TransformToControllerClass(Data.Entity entityConfig, CodeGenerationModel genModel, EntityClass entityClass, ManagerInterface managerInterface)
        {
            var data = new ControllerClass();
            data.Name = $"{entityClass.Name}Controller";
            data.Namespace = $"{genModel.BaseNamespace}.Api.Controllers";
            data.Partial = entityConfig.PartialController;
            data.PrivateConstructor = entityConfig.PrivateControllerConstructor;

            data.WebApiRoutePrefix = entityConfig.WebApiRoutePrefix;

            data.Usings.Add("Microsoft.AspNetCore.Mvc");
            data.Usings.Add("System");
            data.Usings.Add("System.Net");
            data.Usings.Add("Beef");
            data.Usings.Add("Beef.AspNetCore.WebApi");
            data.Usings.Add(entityClass.Namespace);
            data.Usings.Add(managerInterface.Namespace);

            data.Operations = entityConfig.Operations.Select(o =>
            {
                var op = CreateOperation<ControllerOperationModel>(o, entityClass);
                op.WebApiRoute = o.WebApiRoute;

                if (op.IsGet || op.IsDelete)
                {
                    op.WebApiRoute = op.WebApiRoute ?? String.Join('/', op.Parameters.Select(o => $"{{{o.Name}}}"));
                }

                if (op.IsGetColl)
                {
                    op.WebApiRoute = op.WebApiRoute ?? "";
                    op.HasPagingArgs = o.PagingArgs;
                }

                if (op.IsUpdate)
                {
                    op.Parameters.AddRange(entityClass.Properties.Where(o => o.UnqiueKey).Select(o => new OperationParameterModel { Name = o.Name.ToCamelCase(), Type = o.Type, EntityProperty = o.Name }));
                    op.WebApiRoute = op.WebApiRoute ?? String.Join('/', op.Parameters.Skip(1).Select(o => $"{{{o.Name}}}"));
                }

                return op;
            }).ToList();

            return data;
        }

        private static T CreateOperation<T>(Operations config, EntityClass entityClass)
            where T : OperationModelBase, new()
        {
            var op = new T { Name = config.Name ?? config.Type };
            switch (config.Type)
            {
                case "Create":
                    op.IsCreate = true;
                    op.ReturnType = entityClass.Name;
                    op.Parameters = new List<OperationParameterModel> { new OperationParameterModel { Name = "value", Type = entityClass.Name, Mandatory = true, Validator = config.Validator ?? entityClass.Validator } };
                    break;
                case "Get":
                    op.IsGet = true;
                    op.ReturnType = $"{entityClass.Name}?";
                    op.Parameters = entityClass.Properties?.Where(o => o.UnqiueKey).Select(o => new OperationParameterModel { Name = o.Name.ToCamelCase(), Type = o.Type, Mandatory = true, Validator = config.Validator ?? entityClass.Validator }).ToList();
                    break;
                case "Update":
                    op.IsUpdate = true;
                    op.ReturnType = entityClass.Name;
                    op.Parameters = new List<OperationParameterModel> { new OperationParameterModel { Name = "value", Type = entityClass.Name, Mandatory = true, Validator = config.Validator ?? entityClass.Validator } };
                    break;
                case "Delete":
                    op.IsDelete = true;
                    op.ReturnType = null;
                    op.Parameters = entityClass.Properties?.Where(o => o.UnqiueKey).Select(o => new OperationParameterModel { Name = o.Name.ToCamelCase(), Type = o.Type, Mandatory = true, Validator = config.Validator/* ?? entityClass.Validator*/ }).ToList();
                    break;
                case "GetColl":
                    op.IsGetColl = true;
                    op.ReturnType = config.PagingArgs ? $"{entityClass.Name}CollectionResult" : $"{entityClass.Name}Collection";
                    op.Parameters = config.Parameters?.Select(o => new OperationParameterModel { Name = o.Name.ToCamelCase(), Type = o.Type, Validator = o.Validator, Mandatory = o.Mandatory.GetValueOrDefault(true) }).ToList();
                    break;
            }
            return op;
        }

    }
}

#nullable restore