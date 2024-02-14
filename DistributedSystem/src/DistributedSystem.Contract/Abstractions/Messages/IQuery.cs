using DistributedSystem.Contract.Abstractions.Shared;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedSystem.Contract.Abstractions.Messages
{
    public interface IQuery<TResponse> : IRequest<Result<TResponse>>
    { }
}
