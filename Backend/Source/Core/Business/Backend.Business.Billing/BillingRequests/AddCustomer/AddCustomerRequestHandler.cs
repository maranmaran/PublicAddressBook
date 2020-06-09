using System;
using System.Threading;
using System.Threading.Tasks;
using Backend.Library.Payment.Interfaces;
using MediatR;

namespace Backend.Business.Billing.BillingRequests.AddCustomer
{
    public class AddCustomerRequestHandler : IRequestHandler<AddCustomerRequest, string>
    {
        private readonly IPaymentService _paymentService;

        public AddCustomerRequestHandler(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }


        public async Task<string> Handle(AddCustomerRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var customerId = await _paymentService.AddCustomer(request.FullName, request.Email);
                return customerId;
            }
            catch (Exception e)
            {
                throw new Exception(nameof(AddCustomerRequest), e);
            }
        }
    }
}