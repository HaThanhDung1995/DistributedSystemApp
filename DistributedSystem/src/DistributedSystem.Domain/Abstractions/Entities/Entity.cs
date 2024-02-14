﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedSystem.Domain.Abstractions.Entities
{
    public abstract class Entity<T> : IEntity<T>
    {
        public T Id { get; set; }
        public bool IsDeleted { get; protected set; }
    }
}
