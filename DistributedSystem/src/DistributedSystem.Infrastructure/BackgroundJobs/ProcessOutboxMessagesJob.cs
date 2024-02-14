using DistributedSystem.Contract.Abstractions.Messages;
using DistributedSystem.Contract.Services.V1.Product;
using DistributedSystem.Persistence;
using DistributedSystem.Persistence.Outbox;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedSystem.Infrastructure.BackgroundJobs
{
    //DisaalowConcurrentExcution do not allow create multiple instance for the process lifetime 
    [DisallowConcurrentExecution]
    public class ProcessOutboxMessagesJob : IJob
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;
        public ProcessOutboxMessagesJob(ApplicationDbContext dbContext, IPublishEndpoint publishEndpoint)
        {
            _dbContext = dbContext;
            _publishEndpoint = publishEndpoint;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            List<OutboxMessage> messages = await _dbContext
                                                    .Set<OutboxMessage>()
                                                    .Where(m => m.ProcessedOnUtc == null)
                                                    .OrderBy(m => m.OccurredOnUtc)
                                                    .Take(20)
                                                    .ToListAsync(context.CancellationToken);
            foreach (OutboxMessage outboxMessage in messages)
            {
                IDomainEvent? domainEvent = JsonConvert
                .DeserializeObject<IDomainEvent>(
                    outboxMessage.Content,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    });

                if (domainEvent is null)
                {
                    outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
                    continue;
                }
                    

                try
                {
                    switch (domainEvent.GetType().Name)
                    {
                        case nameof(DomainEvent.ProductCreated):
                            var productCreated = JsonConvert.DeserializeObject<DomainEvent.ProductCreated>(
                                        outboxMessage.Content,
                                        new JsonSerializerSettings
                                        {
                                            TypeNameHandling = TypeNameHandling.All
                                        });
                            await _publishEndpoint.Publish(productCreated, context.CancellationToken);
                            break;

                        case nameof(DomainEvent.ProductUpdated):
                            var productUpdated = JsonConvert.DeserializeObject<DomainEvent.ProductUpdated>(
                                        outboxMessage.Content,
                                        new JsonSerializerSettings
                                        {
                                            TypeNameHandling = TypeNameHandling.All
                                        });
                            await _publishEndpoint.Publish(productUpdated, context.CancellationToken);
                            break;

                        case nameof(DomainEvent.ProductDeleted):
                            var productDeleted = JsonConvert.DeserializeObject<DomainEvent.ProductDeleted>(
                                        outboxMessage.Content,
                                        new JsonSerializerSettings
                                        {
                                            TypeNameHandling = TypeNameHandling.All
                                        });
                            await _publishEndpoint.Publish(productDeleted, context.CancellationToken);
                            break;
                        default:
                            break;
                    }

                    outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    outboxMessage.Error = ex.Message;
                }
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
