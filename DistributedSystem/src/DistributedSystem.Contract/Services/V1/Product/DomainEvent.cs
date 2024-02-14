using DistributedSystem.Contract.Abstractions.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedSystem.Contract.Services.V1.Product
{
    public static class DomainEvent
    {
        public record ProductCreated(Guid IdEvent, Guid Id, string Name, decimal Price, string Description) : IDomainEvent, ICommand;
        public record ProductDeleted(Guid IdEvent, Guid Id) : IDomainEvent, ICommand;
        public record ProductUpdated(Guid IdEvent, Guid Id, string Name, decimal Price, string Description) : IDomainEvent, ICommand;
    }
}
