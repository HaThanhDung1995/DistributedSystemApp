using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedSystem.Infrastructure.Consumer.Abstractions
{
    public abstract class Document : IDocument
    {
        public ObjectId Id { get; set; }

        public Guid DocumentId { get; set; } // Id cua SourceMessage: ProductID, CustomerID, OrderID

        public DateTimeOffset CreatedOnUtc => Id.CreationTime;

        public DateTimeOffset? ModifiedOnUtc { get; set; }
    }
}
