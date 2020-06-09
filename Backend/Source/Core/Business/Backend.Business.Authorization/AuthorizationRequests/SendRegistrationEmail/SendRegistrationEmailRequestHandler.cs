using Backend.Domain.Enum;
using Backend.Library.Logging.Interfaces;
using MediatR;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Backend.Business.Email.EmailRequests.RegistrationEmail;

[assembly: InternalsVisibleTo("Backend.BusinessTests")]
namespace Backend.Business.Authorization.AuthorizationRequests.SendRegistrationEmail
{
    public class SendRegistrationEmailRequestHandler : IRequestHandler<SendRegistrationEmailRequest, Unit>
    {
        private readonly ILoggingService _logger;
        private readonly IMediator _mediator;

        public SendRegistrationEmailRequestHandler(ILoggingService logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<Unit> Handle(SendRegistrationEmailRequest request, CancellationToken cancellationToken)
        {
            try
            {
                switch (request.AccountType)
                {
                    case AccountType.User:

                        await _mediator.Send(new RegistrationEmailRequest(request.User), cancellationToken);
                        break;

                    default:
                        throw new NotImplementedException($"This account type does not exist: {request.AccountType}");
                }

                return Unit.Value;
            }
            catch (Exception e)
            {
                var message = $"Could not send registration email for {request.AccountType}";
                await _logger.LogWarning(e, message);
                throw new InvalidOperationException(message, e);
            }
        }


    }
}