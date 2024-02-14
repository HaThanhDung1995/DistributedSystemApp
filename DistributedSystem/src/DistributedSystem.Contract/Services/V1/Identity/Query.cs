using DistributedSystem.Contract.Abstractions.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistributedSystem.Contract.Services.V1.Identity
{
    public static class Query
    {
        public record Login(string Email, string Password) : IQuery<Response.Authenticated>;

        public record Token(string? AccessToken, string? RefreshToken) : IQuery<Response.Authenticated>;
    }
}
