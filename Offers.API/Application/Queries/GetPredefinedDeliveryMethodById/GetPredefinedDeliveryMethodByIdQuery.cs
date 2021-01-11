using Common.Extensions;
using FluentValidation;
using MediatR;
using Offers.API.Application.Dto;

namespace Offers.API.Application.Queries.GetPredefinedDeliveryMethodById
{
    public class GetPredefinedDeliveryMethodByIdQuery : IRequest<PredefinedDeliveryMethodDto>
    {
        public string DeliveryMethodId { get; init; }
    }

    public class GetPredefinedDeliveryMethodByIdQueryValidator : AbstractValidator<GetPredefinedDeliveryMethodByIdQuery>
    {
        public GetPredefinedDeliveryMethodByIdQueryValidator()
        {
            RuleFor(x => x.DeliveryMethodId)
                .IsNotEmptyGuid();
        }
    }
}
