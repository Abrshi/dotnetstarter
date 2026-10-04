namespace starter.Application.Auth.DTOs;

public sealed record SignupResponse(
    int Id,
    string Username,
    string FirstName,
    string MiddleName,
    string LastName,
    string Role,
    DateTime CreatedAt
);