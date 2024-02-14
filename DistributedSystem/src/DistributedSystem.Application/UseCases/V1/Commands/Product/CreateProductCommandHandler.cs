using DistributedSystem.Contract.Abstractions.Messages;
using DistributedSystem.Contract.Abstractions.Shared;
using DistributedSystem.Contract.Services.V1.Product;
using DistributedSystem.Domain.Abstractions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedSystem.Application.UseCases.V1.Commands.Product
{
    public sealed class CreateProductCommandHandler : ICommandHandler<Command.CreateProductCommand>
    {
        private readonly IRepositoryBase<Domain.Entities.Product, Guid> _productRepository;

        public CreateProductCommandHandler(IRepositoryBase<Domain.Entities.Product, Guid> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result> Handle(Command.CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = Domain.Entities.Product.CreateProduct(Guid.NewGuid(), request.Name, request.Price, request.Description);
            _productRepository.Add(product);

            return Result.Success();
        }
    }
}
