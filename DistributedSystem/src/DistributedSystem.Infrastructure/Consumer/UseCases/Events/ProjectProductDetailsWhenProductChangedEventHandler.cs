using DistributedSystem.Contract.Abstractions.Messages;
using DistributedSystem.Contract.Abstractions.Shared;
using DistributedSystem.Contract.Services.V1.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedSystem.Infrastructure.Consumer.UseCases.Events
{
    public class ProjectProductDetailsWhenProductChangedEventHandler
    : ICommandHandler<DomainEvent.ProductCreated>,
    ICommandHandler<DomainEvent.ProductDeleted>,
    ICommandHandler<DomainEvent.ProductUpdated>
    {
        // Repository working with MongoDB
        public async Task<Result> Handle(DomainEvent.ProductCreated request, CancellationToken cancellationToken)
        {
            // Create new a product
            await Task.Delay(1000);

            return Result.Success();
        }

        public async Task<Result> Handle(DomainEvent.ProductDeleted request, CancellationToken cancellationToken)
        {
            // Find and delete product
            await Task.Delay(1000);

            return Result.Success();
        }

        public async Task<Result> Handle(DomainEvent.ProductUpdated request, CancellationToken cancellationToken)
        {
            // Find and update product
            await Task.Delay(1000);

            return Result.Success();
        }

    }
}
