﻿using DistributedSystem.Application.Abstractions;
using DistributedSystem.Contract.JsonConverters;
using DistributedSystem.Infrastructure.Authentication;
using DistributedSystem.Infrastructure.BackgroundJobs;
using DistributedSystem.Infrastructure.Caching;
using DistributedSystem.Infrastructure.DependencyInjection.Options;
using DistributedSystem.Infrastructure.PipeObservers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        public static IServiceCollection AddMasstransitRabbitMQInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var masstransitConfiguration = new MasstransitConfiguration();
            configuration.GetSection(nameof(MasstransitConfiguration)).Bind(masstransitConfiguration);

            var messageBusOption = new MessageBusOptions();
            configuration.GetSection(nameof(MessageBusOptions)).Bind(messageBusOption);
            services.AddMassTransit(cfg =>
            {
                // ===================== Setup for Consumer =====================
                cfg.AddConsumers(Assembly.GetExecutingAssembly()); // Add all of consumers to masstransit instead above command

                // ?? => Configure endpoint formatter. Not configure for producer Root Exchange
                cfg.SetKebabCaseEndpointNameFormatter(); // ??

                cfg.UsingRabbitMq((context, bus) =>
                {
                    bus.Host(masstransitConfiguration.Host, masstransitConfiguration.Port, masstransitConfiguration.VHost, h =>
                    {
                        h.Username(masstransitConfiguration.UserName);
                        h.Password(masstransitConfiguration.Password);
                    });

                    bus.UseMessageRetry(retry
                    => retry.Incremental(
                               retryLimit: messageBusOption.RetryLimit,
                               initialInterval: messageBusOption.InitialInterval,
                               intervalIncrement: messageBusOption.IntervalIncrement));

                    bus.UseNewtonsoftJsonSerializer();

                    bus.ConfigureNewtonsoftJsonSerializer(settings =>
                    {
                        settings.Converters.Add(new TypeNameHandlingConverter(TypeNameHandling.Objects));
                        settings.Converters.Add(new DateOnlyJsonConverter());
                        settings.Converters.Add(new ExpirationDateOnlyJsonConverter());
                        return settings;
                    });

                    bus.ConfigureNewtonsoftJsonDeserializer(settings =>
                    {
                        settings.Converters.Add(new TypeNameHandlingConverter(TypeNameHandling.Objects));
                        settings.Converters.Add(new DateOnlyJsonConverter());
                        settings.Converters.Add(new ExpirationDateOnlyJsonConverter());
                        return settings;
                    });

                    bus.ConnectReceiveObserver(new LoggingReceiveObserver());
                    bus.ConnectConsumeObserver(new LoggingConsumeObserver());
                    bus.ConnectPublishObserver(new LoggingPublishObserver());
                    bus.ConnectSendObserver(new LoggingSendObserver());

                    // Rename for Root Exchange and setup for consumer also
                    bus.MessageTopology.SetEntityNameFormatter(new KebabCaseEntityNameFormatter());

                    // ===================== Setup for Consumer =====================

                    // Importantce to create Echange and Queue
                    bus.ConfigureEndpoints(context);
                });
            });

            return services;
        }
        public static void AddQuartzInfrastructure(this IServiceCollection services)
        {
            services.AddQuartz(configure =>
            {
                var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));

                configure
                    .AddJob<ProcessOutboxMessagesJob>(jobKey)
                    .AddTrigger(
                        trigger =>
                            trigger.ForJob(jobKey)
                                .WithSimpleSchedule(
                                    schedule =>
                                        schedule.WithIntervalInSeconds(100)
                                            .RepeatForever()));

                configure.UseMicrosoftDependencyInjectionJobFactory();
            });

            services.AddQuartzHostedService();
        }
        public static void AddMediatRInfrastructure(this IServiceCollection services)
        => services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly));
    }
}