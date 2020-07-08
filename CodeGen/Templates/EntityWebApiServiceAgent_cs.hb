#nullable enable

{{#each ServiceAgentClass.Usings}}
using {{this}};
{{/each}}

namespace {{ServiceAgentClass.Namespace}}
{
    public class {{ServiceAgentClass.Name}} : WebApiServiceAgentBase<{{ServiceAgentClass.Name}}>
    {
        public {{ServiceAgentClass.Name}}() : base() { }
        public {{ServiceAgentClass.Name}}(HttpClient? httpClient = null, Action<HttpRequestMessage>? beforeRequest = null) : base(httpClient, beforeRequest) { }

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