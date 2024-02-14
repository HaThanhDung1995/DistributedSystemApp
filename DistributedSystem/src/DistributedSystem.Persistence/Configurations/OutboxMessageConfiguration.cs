using DistributedSystem.Persistence.Constants;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DistributedSystem.Persistence.Outbox;

namespace DistributedSystem.Persistence.Configurations
{
    internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.ToTable(TableNames.OutboxMessages);

            builder.HasKey(x => x.Id);
        }
    }
}
