using CodeGen.Data;
using System.Linq;

namespace CodeGen.Models
{
    public class ServiceCollectionExtensionsModelProvider
    {
        public ServiceCollectionExtensionsModelProvider(ICodeGenerationModelProvider domainDataProvider)
        {
            DomainDataProvider = domainDataProvider;
        }

        public ICodeGenerationModelProvider DomainDataProvider { get; set; }

        public object GetData()
        {
            var data = DomainDataProvider.GetData();

            var extensionsModel = new ServiceCollectionExtensionsModel();
            extensionsModel.Namespace = $"{data.BaseNamespace}.Api";
            extensionsModel.EntitiesWithDataClasses = data.Entities.Where(o => o.DataInterface != null && o.DataClass != null).ToList();
            extensionsModel.EntitiesWithDataServiceClasses = data.Entities.Where(o => o.DataServiceInterface != null && o.DataServiceClass != null).ToList();
            extensionsModel.EntitiesWithManagerClasses = data.Entities.Where(o => o.ManagerInterface != null && o.Manager != null).ToList();

            extensionsModel.Usings.Add("Microsoft.Extensions.DependencyInjection");
            if (extensionsModel.EntitiesWithDataClasses.Count > 0)
                extensionsModel.Usings.Add(extensionsModel.EntitiesWithDataClasses.First().DataClass.Namespace);
            if (extensionsModel.EntitiesWithDataServiceClasses.Count > 0)
                extensionsModel.Usings.Add(extensionsModel.EntitiesWithDataServiceClasses.First().DataServiceClass.Namespace);
            if (extensionsModel.EntitiesWithManagerClasses.Count > 0)
                extensionsModel.Usings.Add(extensionsModel.EntitiesWithManagerClasses.First().Manager.Namespace);

            return extensionsModel;
        }
    }
}
