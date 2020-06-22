﻿/*
 * This file is automatically generated; any changes will be lost. 
 */

#nullable enable

{{#each DataServiceClass.Usings}}
using {{this}};
{{/each}}

namespace {{DataServiceClass.Namespace}}
{
    public {{#if DataServiceClass.Partial}}partial {{/if}}class {{DataServiceClass.Name}} : {{DataServiceInterface.Name}}
    {
        {{#if DataServiceClass.Partial}}private{{else}}public{{/if}} {{DataServiceClass.Name}}({{DataInterface.Name}} data)
        {
            _data = data;
        }

        #region Private

        private readonly {{DataInterface.Name}} _data;
        {{#if DataServiceClass.Partial}}
        {{#each DataServiceClass.Operations}}
        {{#if IsCreate}}
        private Func<{{ReturnType}}, Task>? _OnAfter{{Name}}Async;
        {{/if}}
        {{#if IsGet}}
        private Func<{{ReturnType}}{{#each Parameters}}, {{Type}}{{/each}}, Task>? _OnAfter{{Name}}Async;
        {{/if}}
        {{#if IsGetColl}}
        private Func<{{ReturnType}}{{#each Parameters}}, {{Type}}{{/each}}, Task>? _OnAfter{{Name}}Async;
        {{/if}}
        {{#if IsUpdate}}
        private Func<{{ReturnType}}, Task>? _OnAfter{{Name}}Async;
        {{/if}}
        {{#if IsDelete}}
        private Func<{{#each Parameters}}{{Type}}{{#unless @last}}, {{/unless}}{{/each}}, Task> _OnAfter{{Name}}Async;
        {{/if}}
        {{/each}}
        {{/if}}

        #endregion

        {{#each DataServiceClass.Operations}}
        public Task{{#if ReturnType}}<{{ReturnType}}>{{/if}} {{Name}}Async({{#each Parameters}}{{Type}} {{Name}}{{#unless @last}}, {{/unless}}{{/each}})
        {
            return DataSvcInvoker.Default.InvokeAsync(this, async () => 
            {
            {{#if IsCreate}}
                var __result = await _data.{{Name}}Async({{#each Parameters}}Check.NotNull({{Name}}, nameof({{Name}})){{#unless @last}}, {{/unless}}{{/each}}).ConfigureAwait(false);
                {{#if EventAction}}
                await Beef.Events.Event.PublishValueEventAsync(__result, "{{EventSubject}}", "{{EventAction}}").ConfigureAwait(false);;
                {{/if}}
                {{#if ../DataServiceClass.HasCaching}}
                ExecutionContext.Current.CacheSet(__result.UniqueKey, __result);
                {{/if}}
                {{#if ../DataServiceClass.Partial}}
                if (_OnAfter{{Name}}Async != null) await _OnAfter{{Name}}Async(__result).ConfigureAwait(false);
                {{/if}}
                return __result;
            {{/if}}
            {{#if IsGet}}
                {{#if ../DataServiceClass.HasCaching}}
                var __key = new UniqueKey({{#each Parameters}}{{Name}}{{#unless @last}}, {{/unless}}{{/each}});
                if (ExecutionContext.Current.TryGetCacheValue<{{ReturnType}}>(__key, out {{ReturnType}} __val))
                    return __val;

                {{/if}}
                var __result = await _data.{{Name}}Async({{#each Parameters}}{{Name}}{{#unless @last}}, {{/unless}}{{/each}}).ConfigureAwait(false);
                {{#if ../DataServiceClass.HasCaching}}
                ExecutionContext.Current.CacheSet(__key, __result!);
                {{/if}}
                {{#if ../DataServiceClass.Partial}}
                if (_OnAfter{{Name}}Async != null) await _OnAfter{{Name}}Async(__result{{#each Parameters}}, {{Name}}{{/each}}).ConfigureAwait(false);
                {{/if}}
                return __result;
            {{/if}}
            {{#if IsGetColl}}
                var __result = await _data.{{Name}}Async({{#each Parameters}}{{Name}}{{#unless @last}}, {{/unless}}{{/each}}).ConfigureAwait(false);
                {{#if ../DataServiceClass.Partial}}
                if (_OnAfter{{Name}}Async != null) await _OnAfter{{Name}}Async(__result{{#each Parameters}}, {{Name}}{{/each}}).ConfigureAwait(false);
                {{/if}}
                return __result;
            {{/if}}
            {{#if IsUpdate}}
                var __result = await _data.{{Name}}Async({{#each Parameters}}Check.NotNull({{Name}}, nameof({{Name}})){{#unless @last}}, {{/unless}}{{/each}}).ConfigureAwait(false);
                {{#if EventAction}}
                await Beef.Events.Event.PublishValueEventAsync(__result, "{{EventSubject}}", "{{EventAction}}").ConfigureAwait(false);
                {{/if}}
                {{#if ../DataServiceClass.HasCaching}}
                ExecutionContext.Current.CacheSet(__result.UniqueKey, __result);
                {{/if}}
                {{#if ../DataServiceClass.Partial}}
                if (_OnAfter{{Name}}Async != null) await _OnAfter{{Name}}Async(__result).ConfigureAwait(false);
                {{/if}}
                return __result;
            {{/if}}
            {{#if IsDelete}}
                await _data.{{Name}}Async({{#each Parameters}}{{Name}}{{#unless @last}}, {{/unless}}{{/each}}).ConfigureAwait(false);
                {{#if EventAction}}
                await Beef.Events.Event.PublishEventAsync("{{EventSubject}}", "{{EventAction}}"{{#each Parameters}}, {{Name}}{{/each}}).ConfigureAwait(false);
                {{/if}}
                var __key = new UniqueKey({{#each Parameters}}{{Name}}{{#unless @last}}, {{/unless}}{{/each}});
                {{#if ../DataServiceClass.Partial}}
                ExecutionContext.Current.CacheRemove<{{../Name}}>(__key);
                {{/if}}
                {{#if ../DataServiceClass.Partial}}
                if (_OnAfter{{Name}}Async != null) await _OnAfter{{Name}}Async({{#each Parameters}}{{Name}}{{#unless @last}}, {{/unless}}{{/each}}).ConfigureAwait(false);
                {{/if}}
            {{/if}}
            });
        }
        {{#unless @last}}
        
        {{/unless}}
        {{/each}}
    }
}

#nullable restore
