using DistributedSystem.Contract.Abstractions.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DistributedSystem.Contract.Services.V1.Product.Response;

namespace DistributedSystem.Contract.Services.V1.Product
{
    public static class Query
    {
        public record GetProductsQuery() : IQuery<List<ProductResponse>>;
        public record GetProductByIdQuery(Guid Id) : IQuery<ProductResponse>;
    }
}
