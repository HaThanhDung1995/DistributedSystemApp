﻿using DistributedSystem.Application.Behaviors;
using DistributedSystem.Application.Mappers;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedSystem.Application.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfigureMediartApplication(this IServiceCollection services)
        {

            return services
                .AddMediatR(options => options.RegisterServicesFromAssembly(AssemblyReference.Assembly))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(TracingPipelineBehavior<,>))
                .AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionPipelineBehavior<,>))
                .AddValidatorsFromAssembly(Contract.AssemblyReference.Assembly, includeInternalTypes: true)
                ;
        }
        public static IServiceCollection AddConfigureAutoMapperApplication(this IServiceCollection services)
        {

            return services
                .AddAutoMapper(typeof(ServiceProfile))
                ;
        }
    }
}