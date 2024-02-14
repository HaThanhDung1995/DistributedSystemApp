using DistributedSystem.Contract.Abstractions.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedSystem.Contract.Services.V1.Identity
{
    public static class Command
    {
        public record Revoke(string AccessToken) : ICommand;
    }
}
