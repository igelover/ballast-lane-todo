using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using Ballast.Todo.Infrastructure.Database;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver;

namespace Ballast.Todo.Infrastructure.Identity
{
    [ExcludeFromCodeCoverage]
    public class TokenAuthorizationHandler : AuthorizationHandler<ValidTokenRequirement>
    {
        private readonly IDataContext _context;

        public TokenAuthorizationHandler(IDataContext context)
        {
            _context = context;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ValidTokenRequirement requirement)
        {
            var tokenId = context.User.Claims.FirstOrDefault(claim => claim.Type == JwtRegisteredClaimNames.Jti)?.Value;
            var userId = context.User.Identity?.Name;

            var userSessions = await _context.Sessions.FindAsync(
                                s => s.UserId == userId
                                && s.TokenId == tokenId
                                && s.Active);
            var activeSession = userSessions.FirstOrDefault();

            if (activeSession != null)
            {
                context.Succeed(requirement);
            }
        }
    }
}
