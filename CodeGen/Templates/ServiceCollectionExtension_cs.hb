﻿/*
 * This file is automatically generated; any changes will be lost. 
 */

#nullable enable

{{#each Usings}}
using {{this}};
{{/each}}

namespace {{Namespace}}
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddEntityDataComponents(this IServiceCollection services)
        {
            {{#each EntitiesWithDataClasses}}
            services.AddTransient<{{DataInterface.Name}}, {{DataClass.Name}}>();
            {{/each}}

            return services;
        }

        public static IServiceCollection AddEntityDataServiceComponents(this IServiceCollection services)
        {
            {{#each EntitiesWithDataServiceClasses}}
            services.AddTransient<{{DataServiceInterface.Name}}, {{DataServiceClass.Name}}>();
            {{/each}}

            return services;
        }

        public static IServiceCollection AddEntityManagerComponents(this IServiceCollection services)
        {
            {{#each EntitiesWithManagerClasses}}
            services.AddTransient<{{ManagerInterface.Name}}, {{Manager.Name}}>();
            {{/each}}

            return services;
        }
    }
}

#nullable restore
