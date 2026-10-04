using MediatR;
using starter.Application.Auth.DTOs;
using starter.Application.Common;

namespace starter.Application.Auth.Commands.Signup;

public sealed record SignupCommand(
    SignupRequest Request
) : IRequest<Result<SignupResponse>>;