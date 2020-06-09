using Backend.Business.Billing.BillingRequests.GetSubscription;
using Backend.Infrastructure.Exceptions;
using Backend.Library.Payment.Interfaces;
using Moq;
using Stripe;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Billing
{
    public class GetSubscriptionTests
    {
        [Fact]
        public async Task GetSubscriptionHandler_ValidRequest_Gets()
        {
            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(x => x.GetCustomerSubscription(It.IsAny<string>())).ReturnsAsync(() => new Subscription() { Id = "Id" });

            var handler = new GetSubscriptionRequestHandler(paymentServiceMock.Object);

            var request = new GetSubscriptionRequest("test");
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal("Id", result.Id);
        }

        [Fact]
        public async Task GetSubscriptionHandler_InValidRequest_Throws()
        {
            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(x => x.GetCustomerSubscription(It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            var handler = new GetSubscriptionRequestHandler(paymentServiceMock.Object);

            var request = new GetSubscriptionRequest("test");
            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, CancellationToken.None));
        }
    }
}
