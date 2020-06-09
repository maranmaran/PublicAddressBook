using AutoMapper;
using Backend.Business.Billing.BillingRequests.Subscribe;
using Backend.Library.Payment.Interfaces;
using Backend.Library.Payment.Models;
using Moq;
using Stripe;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Billing
{
    public class SubscribeTests
    {
        private readonly IMapper _mapper;

        public SubscribeTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<SubscribeRequest, PaymentModel>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task SubscribeHandler_ValidRequest_Subscribes()
        {
            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(x => x.AddSubscription(It.IsAny<PaymentModel>())).ReturnsAsync(() => new Subscription() { Id = "Id" });

            var handler = new SubscribeRequestHandler(paymentServiceMock.Object, _mapper);

            var request = new SubscribeRequest() { CustomerId = "test", PlanId = "test" };
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal("Id", result.Id);
        }

        [Fact]
        public async Task SubscribeHandler_InValidRequest_Throws()
        {
            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(x => x.AddSubscription(It.IsAny<PaymentModel>()))
                .ThrowsAsync(new Exception());

            var handler = new SubscribeRequestHandler(paymentServiceMock.Object, _mapper);

            var request = new SubscribeRequest() { CustomerId = "test", PlanId = "test" };
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.Handle(request, CancellationToken.None));
        }
    }
}
