using DistributedSystem.Application.Abstractions;
using DistributedSystem.Contract.Abstractions.Messages;
using DistributedSystem.Contract.Abstractions.Shared;
using DistributedSystem.Contract.Services.V1.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DistributedSystem.Application.UseCases.V1.Commands.Identity
{
    public class RevokeTokenCommandHandler : ICommandHandler<Command.Revoke>
    {
        private readonly ICacheService _cacheService;
        private readonly IJwtTokenService _jwtTokenService;

        public RevokeTokenCommandHandler( IJwtTokenService jwtTokenService, ICacheService cacheService)
        {
            
            _jwtTokenService = jwtTokenService;
            _cacheService = cacheService;
        }

        public async Task<Result> Handle(Command.Revoke request, CancellationToken cancellationToken)
        {
            var AccessToken = request.AccessToken;
            var principal = _jwtTokenService.GetPrincipalFromExpiredToken(AccessToken);
            var emailKey = principal.FindFirstValue(ClaimTypes.Email).ToString();

            var authenticated = await _cacheService.GetAsync<Response.Authenticated>(emailKey);
            if (authenticated is null)
                throw new Exception("Can not get value from Redis");

            await _cacheService.RemoveAsync(emailKey, cancellationToken);

            return Result.Success();
        }
    }
}
