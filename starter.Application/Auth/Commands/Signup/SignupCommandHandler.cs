using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using starter.Application.Auth.DTOs;
using starter.Application.Common;
using starter.Domain.Entities;
using starter.Infrastructure.Data;

namespace starter.Application.Auth.Commands.Signup;

public sealed class SignupCommandHandler
    : IRequestHandler<SignupCommand, Result<SignupResponse>>
{
    private readonly StarterDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;

    public SignupCommandHandler(
        StarterDbContext dbContext,
        IPasswordHasher<User> passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public async Task<Result<SignupResponse>> Handle(
        SignupCommand command,
        CancellationToken cancellationToken)
    {
        var request = command.Request;

        var username = request.Username.Trim();

        var existingUser = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(
                u => u.Username == username,
                cancellationToken);

        if (existingUser is not null)
        {
            return Result<SignupResponse>.Failure(
                "Username is already taken.",
                StatusCodes.Status409Conflict);
        }

        var user = new User
        {
            Username = username,
            FirstName = request.FirstName.Trim(),
            MiddleName = request.MiddleName.Trim(),
            LastName = request.LastName.Trim(),
            Role = Domain.Enums.UserRole.User,
            CreatedAt = DateTime.UtcNow
        };

        user.PasswordHash = _passwordHasher.HashPassword(
            user,
            request.Password);

        _dbContext.Users.Add(user);

        await _dbContext.SaveChangesAsync(cancellationToken);

        var response = new SignupResponse(
            user.Id,
            user.Username,
            user.FirstName,
            user.MiddleName,
            user.LastName,
            user.Role.ToString(),
            user.CreatedAt);

        return Result<SignupResponse>.Success(
            response,
            StatusCodes.Status201Created);
    }
}