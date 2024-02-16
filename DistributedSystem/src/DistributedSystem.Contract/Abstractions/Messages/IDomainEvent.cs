﻿
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedSystem.Contract.Abstractions.Messages
{
    [ExcludeFromTopology]
    public interface IDomainEvent 
    {
        public Guid IdEvent { get; init; }
    }
}
