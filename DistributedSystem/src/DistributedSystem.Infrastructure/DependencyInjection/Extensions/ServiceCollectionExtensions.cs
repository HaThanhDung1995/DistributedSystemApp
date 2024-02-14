using DistributedSystem.Application.Abstractions;
using DistributedSystem.Infrastructure.Authentication;
using DistributedSystem.Infrastructure.Caching;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedSystem.Infrastructure.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServicesInfrastructure(this IServiceCollection services)
        => services
            .AddTransient<IJwtTokenService, JwtTokenService>()
            .AddTransient<ICacheService, CacheService>()
            ;
        public static void AddRedisInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(redisOptions =>
            {
                var connectionString = configuration.GetConnectionString("Redis");
                redisOptions.Configuration = connectionString;
            });
        }
    }
}
