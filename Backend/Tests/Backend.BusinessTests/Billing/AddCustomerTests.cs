using Backend.Business.Billing.BillingRequests.AddCustomer;
using Backend.Library.Payment.Interfaces;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Billing
{
    public class AddCustomerTests
    {
        [Fact]
        public async Task AddCustomerHandler_ValidRequest_Adds()
        {
            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(x => x.AddCustomer(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(() => "id");

            var handler = new AddCustomerRequestHandler(paymentServiceMock.Object);

            var request = new AddCustomerRequest("fullname", "email");
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal("id", result);
        }

        [Fact]
        public async Task AddCustomerHandler_InValidRequest_Throws()
        {
            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(x => x.AddCustomer(It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            var handler = new AddCustomerRequestHandler(paymentServiceMock.Object);

            var request = new AddCustomerRequest("fullname", "email");
            await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
        }
    }
}
