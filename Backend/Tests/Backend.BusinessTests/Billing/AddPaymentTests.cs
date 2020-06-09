using AutoMapper;
using Backend.Business.Billing.BillingRequests.AddPayment;
using Backend.Infrastructure.Exceptions;
using Backend.Library.Payment.Interfaces;
using Backend.Library.Payment.Models;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Billing
{
    public class AddPaymentTests
    {
        private readonly IMapper _mapper;

        public AddPaymentTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AddPaymentRequest, PaymentOption>();
            });
            _mapper = config.CreateMapper();
        }

        [Fact]
        public async Task AddPaymentHandler_ValidRequest_Adds()
        {
            var paymentServiceMock = new Mock<IPaymentService>();

            var handler = new AddPaymentRequestHandler(paymentServiceMock.Object, _mapper);

            var request = new AddPaymentRequest()
            {
                CustomerId = "test",
                Token = "test"
            };
            await handler.Handle(request, CancellationToken.None);
        }

        [Fact]
        public async Task AddPaymentHandler_InValidRequest_Throws()
        {
            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(x => x.AddPaymentOption(It.IsAny<PaymentOption>())).ThrowsAsync(new Exception());

            var handler = new AddPaymentRequestHandler(paymentServiceMock.Object, _mapper);

            var request = new AddPaymentRequest()
            {
                CustomerId = "test",
                Token = "test"
            };

            await Assert.ThrowsAsync<CreateFailureException>(() => handler.Handle(request, CancellationToken.None));
        }

        [Fact]
        public async Task AddPaymentValidator_NotEmptyNullWhitespace_Valid()
        {
            var validator = new AddPaymentRequestValidator();
            var request = new AddPaymentRequest()
            {
                Token = "token",
                CustomerId = "customerId"
            };

            Assert.True((await validator.ValidateAsync(request)).IsValid);
        }

        [Fact]
        public async Task AddPaymentValidator_NotEmptyNullWhitespace_InValid()
        {
            var validator = new AddPaymentRequestValidator();
            var request = new AddPaymentRequest()
            {
                Token = " ",
                CustomerId = "customerId"
            };
            var request2 = new AddPaymentRequest()
            {
                Token = "token",
                CustomerId = " "
            };
            var request3 = new AddPaymentRequest()
            {
                Token = "",
                CustomerId = "customerId"
            };
            var request4 = new AddPaymentRequest()
            {
                Token = "token",
                CustomerId = ""
            };
            var request5 = new AddPaymentRequest()
            {
                Token = null,
                CustomerId = "customerId"
            };
            var request6 = new AddPaymentRequest()
            {
                Token = "token",
                CustomerId = null
            };

            Assert.False((await validator.ValidateAsync(request)).IsValid);
            Assert.False((await validator.ValidateAsync(request2)).IsValid);
            Assert.False((await validator.ValidateAsync(request3)).IsValid);
            Assert.False((await validator.ValidateAsync(request4)).IsValid);
            Assert.False((await validator.ValidateAsync(request5)).IsValid);
            Assert.False((await validator.ValidateAsync(request6)).IsValid);
        }
    }
}
