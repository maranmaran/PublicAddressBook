using Backend.Business.Billing.BillingRequests.CancelSubscription;
using Backend.Library.Payment.Interfaces;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Billing
{
    public class CancelSubscriptionTests
    {
        [Fact]
        public async Task CancelSubscriptionHandler_ValidRequest_Cancels()
        {
            var paymentServiceMock = new Mock<IPaymentService>();

            var handler = new CancelSubscriptionRequestHandler(paymentServiceMock.Object);

            var request = new CancelSubscriptionRequest("test");
            await handler.Handle(request, CancellationToken.None);
        }

        [Fact]
        public async Task CancelSubscriptionHandler_InValidRequest_Throws()
        {
            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(x => x.CancelSubscription(It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            var handler = new CancelSubscriptionRequestHandler(paymentServiceMock.Object);

            var request = new CancelSubscriptionRequest("test");
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(request, CancellationToken.None));
        }
    }
}
