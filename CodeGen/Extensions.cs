#nullable enable

using CodeGen.Generators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace CodeGen
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGenerators(this IServiceCollection services)
        {
            foreach (var item in services.Where(o => typeof(IGenerator).IsAssignableFrom(o.ServiceType)).ToArray())
                services.AddTransient(typeof(IGenerator), item.ServiceType);
            return services;
        }

        public static IServiceCollection AddOption<T>(this IServiceCollection services, Func<IServiceProvider, IConfiguration, T> implementationFactory)
            where T : class
        {
            services.AddSingleton<T>(sp => implementationFactory(sp, sp.GetService<IConfiguration>()));
            return services;
        }

        public static T BindConfig<T>(this IConfiguration configuration, string key, T instance)
        {
            configuration.Bind(key, instance);
            return instance;
        }
    }

    public static class Extensions
    {
        public static bool HasValue(this string? value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// The <see cref="Regex"/> expression pattern for split strings into words.
        /// </summary>
        public const string WordSplitPattern = "([a-z](?=[A-Z])|[A-Z](?=[A-Z][a-z]))";

        public static string? ToCamelCase(this string? text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            return char.ToLower(text[0], CultureInfo.InvariantCulture) + text.Substring(1);
        }

        public static string? ToSentenceCase(this string? text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            var s = Regex.Replace(text, WordSplitPattern, "$1 "); // Split the string into words.
            s = s.Replace("E Tag", "ETag", StringComparison.InvariantCulture); // Special case where we will put back together.
            return char.ToUpper(s[0], CultureInfo.InvariantCulture) + s.Substring(1); // Make sure the first character is always upper case.
        }

        public static string ToDisplayName(this string? text)
        {
            var dn = text.ToSentenceCase() ?? "";
            var parts = dn.Split(' ');
            if (parts.Length == 1)
                return (parts[0] == "Id") ? "Identifier" : dn;

            if (parts.Last() != "Id")
                return dn;

            var parts2 = new string[parts.Length - 1];
            Array.Copy(parts, parts2, parts.Length - 1);
            return string.Join(" ", parts2);
        }
    }
}

#nullable restore