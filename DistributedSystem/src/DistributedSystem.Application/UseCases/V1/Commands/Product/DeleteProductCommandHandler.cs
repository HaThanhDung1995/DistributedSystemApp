using DistributedSystem.Contract.Abstractions.Messages;
using DistributedSystem.Contract.Abstractions.Shared;
using DistributedSystem.Contract.Services.V1.Product;
using DistributedSystem.Domain.Abstractions.Repositories;
using DistributedSystem.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedSystem.Application.UseCases.V1.Commands.Product
{
    public sealed class DeleteProductCommandHandler : ICommandHandler<Command.DeleteProductCommand>
    {
        private readonly IRepositoryBase<DistributedSystem.Domain.Entities.Product, Guid> _productRepository;

        public DeleteProductCommandHandler(IRepositoryBase<DistributedSystem.Domain.Entities.Product, Guid> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result> Handle(Command.DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.FindByIdAsync(request.Id) ?? throw new ProductException.ProductNotFoundException(request.Id);
            product.Delete();
            _productRepository.Remove(product);

            return Result.Success();
        }
    }
}
