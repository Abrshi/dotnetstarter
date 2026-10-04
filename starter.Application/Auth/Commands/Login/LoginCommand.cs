using MediatR;
using starter.Application.Auth.DTOs;
using starter.Application.Common;

namespace starter.Application.Auth.Commands.Login;

public sealed record LoginCommand(
    LoginRequest Request
) : IRequest<Result<LoginResponse>>;