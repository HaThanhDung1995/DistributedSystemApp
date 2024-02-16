using DistributedSystem.Contract.Abstractions.Messages;
using DistributedSystem.Contract.Abstractions.Shared;
using DistributedSystem.Contract.Services.V1.Product;
using DistributedSystem.Infrastructure.Consumer.Abstractions.Repositories;
using DistributedSystem.Infrastructure.Consumer.Models;
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
        private readonly IMongoRepository<ProductProjection> _productRepository;
        public ProjectProductDetailsWhenProductChangedEventHandler(IMongoRepository<ProductProjection> productRepository)
        {
            _productRepository = productRepository;
        }

        // Repository working with MongoDB
        public async Task<Result> Handle(DomainEvent.ProductCreated request, CancellationToken cancellationToken)
        {
            // Create new a product
            var product = new ProductProjection()
            {
                DocumentId = request.Id,
                Name = request.Name,
                Price = request.Price,
                Description = request.Description,
            };
            // Create new a product
            await _productRepository.InsertOneAsync(product);

            return Result.Success();
        }

        public async Task<Result> Handle(DomainEvent.ProductDeleted request, CancellationToken cancellationToken)
        {
            // Find and delete product
            var product = await _productRepository.FindOneAsync(p => p.DocumentId == request.Id)
            ?? throw new ArgumentNullException();
            await _productRepository.DeleteOneAsync(p => p.DocumentId == request.Id);
            return Result.Success();
        }

        public async Task<Result> Handle(DomainEvent.ProductUpdated request, CancellationToken cancellationToken)
        {
            // Find and update product
            var product = await _productRepository.FindOneAsync(p => p.DocumentId == request.Id)
           ?? throw new ArgumentNullException();
            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.ModifiedOnUtc = DateTime.UtcNow;

            await _productRepository.ReplaceOneAsync(product);

            return Result.Success();
        }

    }
}
