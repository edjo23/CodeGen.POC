#nullable enable

{{#each ServiceAgentClass.Usings}}
using {{this}};
{{/each}}

namespace {{ServiceAgentClass.Namespace}}
{
    public partial class {{ServiceAgentClass.Name}} : WebApiServiceAgentBase<{{ServiceAgentClass.Name}}>
    {
        public {{ServiceAgentClass.Name}}(IOptionsMonitor<ServiceAgentOptions> namedOptionsAccessor)
            : base(namedOptionsAccessor.Get(typeof({{ServiceAgentClass.Name}}).FullName).HttpClient, namedOptionsAccessor.Get(nameof({{ServiceAgentClass.Name}})).BeforeRequest) { }
        {{#each ServiceAgentClass.Operations}}
        {{#if IsGet}}

        public async Task<WebApiAgentResult<{{ReturnType}}>> {{Name}}Async({{#each Parameters}}{{Type}} {{Name}}, {{/each}}WebApiRequestOptions? requestOptions = null)
        {
            return await base.GetAsync<{{ReturnType}}>("{{WebApiRoute}}", requestOptions: requestOptions,
                args: new WebApiArg{{#each Parameters}}{{#if @first}}[] { {{/if}}new WebApiArg<{{Type}}>("{{Name}}", {{Name}}){{#unless @last}}, {{else}} }{{/unless}}{{else}}[0]{{/each}});
        }
        {{/if}}
        {{#if IsGetColl}}

        public async Task<WebApiAgentResult<{{ReturnType}}>> {{Name}}Async({{#each Parameters}}{{Type}} {{Name}}, {{/each}}WebApiRequestOptions? requestOptions = null)
        {
            return await base.GetAsync<{{ReturnType}}>("{{WebApiRoute}}", requestOptions: requestOptions,
                args: new WebApiArg{{#each Parameters}}{{#if @first}}[] { {{/if}}new WebApiArg<{{Type}}>("{{Name}}", {{Name}}){{#unless @last}}, {{else}} }{{/unless}}{{else}}[0]{{/each}});
        }
        {{/if}}
        {{/each}}
    }
}

#nullable restore