using Backend.Business.Billing.BillingRequests.GetPlans;
using Backend.Infrastructure.Exceptions;
using Backend.Library.Payment.Interfaces;
using Moq;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Billing
{
    public class GetPlansTests
    {
        [Fact]
        public async Task GetPlansHandler_ValidRequest_Gets()
        {
            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(x => x.GetAvailablePlans(It.IsAny<bool>()))
                .ReturnsAsync(() => new List<Plan>() { new Plan(), new Plan() });

            var handler = new GetPlansRequestHandler(paymentServiceMock.Object);

            var request = new GetPlansRequest();
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetPlansHandler_InValidRequest_Throws()
        {
            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(x => x.GetAvailablePlans(It.IsAny<bool>()))
                .ThrowsAsync(new Exception());

            var handler = new GetPlansRequestHandler(paymentServiceMock.Object);

            var request = new GetPlansRequest();
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, CancellationToken.None));
        }
    }
}
