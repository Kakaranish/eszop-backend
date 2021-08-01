using Common.Utilities.Extensions;
using Identity.Domain.Aggregates.SellerInfoAggregate;
using Identity.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.Application.Commands.CreateOrUpdateSellerInfo
{
    public class CreateOrUpdateSellerInfoCommandHandler : IRequestHandler<CreateOrUpdateSellerInfoCommand>
    {
        private readonly HttpContext _httpContext;
        private readonly ISellerInfoRepository _sellerInfoRepository;

        public CreateOrUpdateSellerInfoCommandHandler(IHttpContextAccessor httpContextAccessor,
            ISellerInfoRepository sellerInfoRepository)
        {
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
            _sellerInfoRepository = sellerInfoRepository ?? throw new ArgumentNullException(nameof(sellerInfoRepository));
        }

        public async Task<Unit> Handle(CreateOrUpdateSellerInfoCommand request, CancellationToken cancellationToken)
        {
            var userId = _httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var sellerInfo = await _sellerInfoRepository.GetByUserIdAsync(userId);
            var sellerInfoExists = sellerInfo != null;

            if (!sellerInfoExists)
            {
                sellerInfo = new SellerInfo(userId, request.ContactEmail, request.PhoneNumber, request.BankAccountNumber);
                if (request.AdditionalInfo != null) sellerInfo.SetAdditionalInfo(request.AdditionalInfo);
                _sellerInfoRepository.Add(sellerInfo);
            }
            else
            {
                if (request.ContactEmail != null) sellerInfo.SetContactEmail(request.ContactEmail);
                if (request.PhoneNumber != null) sellerInfo.SetPhoneNumber(request.PhoneNumber);
                if (request.BankAccountNumber != null) sellerInfo.SetBankAccountNumber(request.BankAccountNumber);
                if (request.AdditionalInfo != null) sellerInfo.SetAdditionalInfo(request.AdditionalInfo);
            }

            if (sellerInfoExists) _sellerInfoRepository.Update(sellerInfo);

            await _sellerInfoRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}
