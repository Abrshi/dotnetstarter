namespace starter.Application.Auth.DTOs;

public sealed record SignupRequest(
    string Username,
    string FirstName,
    string MiddleName,
    string LastName,
    string Password
);