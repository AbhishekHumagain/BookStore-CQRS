﻿namespace BookStore.Application.Identity.Commands.Register;

using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using Common;
using MediatR;

public class UserRegisterCommand : UserRegisterRequestModel, IRequest<Result<UserResponseModel>>
{
    public UserRegisterCommand(
        string fullName,
        string email,
        string password,
        string confirmPassword)
        : base(fullName, email, password)
        => this.ConfirmPassword = confirmPassword;

    public string ConfirmPassword { get; }

    public class UserRegisterCommandHandler : IRequestHandler<UserRegisterCommand, Result<UserResponseModel>>
    {
        private readonly IIdentity identity;

        public UserRegisterCommandHandler(IIdentity identity)
            => this.identity = identity;

        public async Task<Result<UserResponseModel>> Handle(
            UserRegisterCommand request,
            CancellationToken cancellationToken)
            => await this.identity.Register(request);
    }
}