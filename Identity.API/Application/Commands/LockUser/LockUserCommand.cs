using Common.Extensions;
using FluentValidation;
using MediatR;
using System;
using System.Globalization;

namespace Identity.API.Application.Commands.LockUser
{
    public class LockUserCommand : IRequest
    {
        public const string LockUntilFormat = "yyyy-MM-ddTHH:mm";

        public string UserId { get; set; }
        public string LockUntil { get; set; }
    }

    public class LockUserCommandValidator : AbstractValidator<LockUserCommand>
    {
        public LockUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .IsGuid();

            RuleFor(x => x.LockUntil)
                .Must(lockUntilStr =>
                {
                    if (!DateTime.TryParseExact(lockUntilStr, LockUserCommand.LockUntilFormat,
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out var lockUntil))
                    {
                        return false;
                    }
                    if (lockUntil < DateTime.UtcNow.AddSeconds(10))
                    {
                        return false;
                    }

                    return true;
                })
                .WithMessage($"Must have {LockUserCommand.LockUntilFormat} format and cannot be in past");
        }
    }
}
