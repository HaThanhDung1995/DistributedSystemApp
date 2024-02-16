using DistributedSystem.Contract.Abstractions.Shared;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedSystem.Contract.Abstractions.Messages
{
    [ExcludeFromTopology]
    public interface ICommand : IRequest<Result>
    {
    }
    [ExcludeFromTopology]
    public interface ICommand<TResponse> : IRequest<Result<TResponse>>
    {
    }
}
