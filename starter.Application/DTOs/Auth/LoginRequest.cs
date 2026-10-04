namespace starter.Application.Auth.DTOs;

public sealed record LoginRequest(
    string Username,
    string Password
);