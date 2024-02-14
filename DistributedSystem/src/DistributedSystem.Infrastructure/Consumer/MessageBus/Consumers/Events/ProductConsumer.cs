using DistributedSystem.Contract.Services.V1.Product;
using DistributedSystem.Infrastructure.Consumers.Abstractions.Messages;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedSystem.Infrastructure.Consumer.MessageBus.Consumers.Events
{
    public static class ProductConsumer
    {
        public class ProductCreatedConsumer : Consumer<DomainEvent.ProductCreated>
        {
            public ProductCreatedConsumer(ISender sender) : base(sender)
            {
            }
        }

        public class ProductDeletedConsumer : Consumer<DomainEvent.ProductDeleted>
        {
            public ProductDeletedConsumer(ISender sender) : base(sender)
            {
            }
        }

        public class ProductUpdatedConsumer : Consumer<DomainEvent.ProductUpdated>
        {
            public ProductUpdatedConsumer(ISender sender) : base(sender)
            {
            }
        }
    }
}
