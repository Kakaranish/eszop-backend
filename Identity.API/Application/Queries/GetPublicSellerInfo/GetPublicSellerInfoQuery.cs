using Common.Utilities.Extensions;
using FluentValidation;
using Identity.API.Application.Dto;
using MediatR;

namespace Identity.API.Application.Queries.GetPublicSellerInfo
{
    public class GetPublicSellerInfoQuery : IRequest<PublicSellerInfoDto>
    {
        public string SellerId { get; init; }
    }

    public class GetPublicSellerInfoQueryValidator : AbstractValidator<GetPublicSellerInfoQuery>
    {
        public GetPublicSellerInfoQueryValidator()
        {
            RuleFor(x => x.SellerId)
                .IsNotEmptyGuid();
        }
    }
}
