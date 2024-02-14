using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DistributedSystem.Infrastructure.DependencyInjection.Extensions
{
    internal static class NameFormatterExtensions
    {
        public static string ToKebabCaseString(this MemberInfo member)
            => KebabCaseEndpointNameFormatter.Instance.SanitizeName(member.Name);
    }

    internal class KebabCaseEntityNameFormatter : IEntityNameFormatter
    {
        public string FormatEntityName<T>()
            => typeof(T).ToKebabCaseString();
    }
}
