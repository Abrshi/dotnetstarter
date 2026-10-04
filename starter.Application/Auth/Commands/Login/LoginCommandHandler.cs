using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using starter.Application.Auth.DTOs;
using starter.Application.Common;
using starter.Domain.Entities;
using starter.Infrastructure.Data;

namespace starter.Application.Auth.Commands.Login;

public sealed class LoginCommandHandler
    : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly StarterDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;

    public LoginCommandHandler(
        StarterDbContext dbContext,
        IPasswordHasher<User> passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<LoginResponse>> Handle(
        LoginCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;

        var username = request.Username.Trim();

        var user = await _dbContext.Users
            .FirstOrDefaultAsync(
                u => u.Username == username,
                cancellationToken);

        if (user is null)
        {
            return Result<LoginResponse>.Failure(
                "Invalid username or password.",
                StatusCodes.Status401Unauthorized);
        }

        var passwordResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password);

        if (passwordResult == PasswordVerificationResult.Failed)
        {
            return Result<LoginResponse>.Failure(
                "Invalid username or password.",
                StatusCodes.Status401Unauthorized);
        }

        var response = new LoginResponse(
            user.Id,
            user.Username,
            user.FirstName,
            user.MiddleName,
            user.LastName,
            user.Role.ToString(),
            user.CreatedAt);

        return Result<LoginResponse>.Success(response);
    }
}