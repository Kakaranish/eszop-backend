using Common.Extensions;
using FluentValidation;
using Identity.API.Domain;
using MediatR;

namespace Identity.API.Application.Queries.GetSellerInfo
{
    public class GetSellerInfoQuery : IRequest<SellerInfo>
    {
        public string SellerId { get; init; }
    }

    public class GetSellerInfoQueryValidator : AbstractValidator<GetSellerInfoQuery>
    {
        public GetSellerInfoQueryValidator()
        {
            RuleFor(x => x.SellerId)
                .IsNotEmptyGuid();
        }
    }
}
