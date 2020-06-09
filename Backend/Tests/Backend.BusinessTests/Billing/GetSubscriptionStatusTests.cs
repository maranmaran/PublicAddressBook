using Backend.Business.Billing.BillingRequests.GetSubscriptionStatus;
using Backend.Infrastructure.Exceptions;
using Backend.Library.Payment.Enums;
using Backend.Library.Payment.Interfaces;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Billing
{
    public class GetSubscriptionStatusTests
    {
        [Fact]
        public async Task GetSubscriptionStatusHandler_ValidRequest_Gets()
        {
            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(x => x.GetCustomerSubscriptionStatus(It.IsAny<string>())).ReturnsAsync(() => SubscriptionStatus.Active);

            var handler = new GetSubscriptionStatusRequestHandler(paymentServiceMock.Object);

            var request = new GetSubscriptionStatusRequest("test");
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(SubscriptionStatus.Active, result);
        }

        [Fact]
        public async Task GetSubscriptionStatusHandler_InValidRequest_Throws()
        {
            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(x => x.GetCustomerSubscriptionStatus(It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            var handler = new GetSubscriptionStatusRequestHandler(paymentServiceMock.Object);

            var request = new GetSubscriptionStatusRequest("test");
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, CancellationToken.None));
        }
    }
}
