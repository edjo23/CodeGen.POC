#nullable enable

using System;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace CodeGen.Data
{
    public interface ICodeGenerationDataProvider
    {
        CodeGenerationData GetData();
    }

    public class CodeGenerationDataOptions
    {
        public string? FilePath { get; set; }
    }

    public class CodeGenerationDataProvider : ICodeGenerationDataProvider
    {
        public CodeGenerationDataProvider(CodeGenerationDataOptions options)
        {
            _options = options;
            _data = new Lazy<CodeGenerationData>(() => LoadData(), true);
        }

        private readonly CodeGenerationDataOptions _options;
        private Lazy<CodeGenerationData> _data;

        public virtual CodeGenerationData GetData()
        {
            return _data.Value;
        }

        private CodeGenerationData LoadData()
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .IgnoreUnmatchedProperties()
                .Build();

            return deserializer.Deserialize<Data.CodeGenerationData>(File.ReadAllText(_options.FilePath ?? "Data.yaml"));
        }
    }
}

#nullable restore