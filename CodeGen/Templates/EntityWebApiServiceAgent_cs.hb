#nullable enable

{{#each ServiceAgentClass.Usings}}
using {{this}};
{{/each}}

namespace {{ServiceAgentClass.Namespace}}
{
    public class {{ServiceAgentClass.Name}} : WebApiServiceAgentBase<{{ServiceAgentClass.Name}}>
    {
        public {{ServiceAgentClass.Name}}(IOptionsMonitor<ServiceAgentOptions> namedOptionsAccessor)
            : base(namedOptionsAccessor.Get(typeof({{ServiceAgentClass.Name}}).FullName).HttpClient, namedOptionsAccessor.Get(nameof({{ServiceAgentClass.Name}})).BeforeRequest) { }

        {{#each ServiceAgentClass.Operations}}
        {{#if IsGetColl}}
        {{#unless HasPagingArgs}}
        public async Task<WebApiAgentResult<{{ReturnType}}>> {{Name}}Async()
        {
            return await base.GetAsync<{{ReturnType}}>("{{WebApiRoute}}", requestOptions: null,
                args: Array.Empty<WebApiArg>());
        }
        {{/unless}}
        {{/if}}
        {{/each}}
    }
}

#nullable restore