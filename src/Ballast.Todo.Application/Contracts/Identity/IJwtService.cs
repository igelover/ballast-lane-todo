﻿namespace Ballast.Todo.Application.Contracts.Identity
{
    public interface IJwtService
    {
        (string tokenId, string token) GenerateJwtFor(string targetId, string role);
    }
}
