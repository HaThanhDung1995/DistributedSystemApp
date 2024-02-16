using DistributedSystem.Infrastructure.Consumer.Abstractions;
using DistributedSystem.Infrastructure.Consumer.Attributes;
using DistributedSystem.Infrastructure.Consumer.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedSystem.Infrastructure.Consumer.Models
{
    [BsonCollection(TableNames.Event)]
    public class EventProjection : Document
    {
        public Guid EventId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
