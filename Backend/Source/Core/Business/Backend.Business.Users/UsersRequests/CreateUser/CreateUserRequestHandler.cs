using AutoMapper;
using Backend.Business.Authorization.AuthorizationRequests.SendRegistrationEmail;
using Backend.Business.Billing.BillingRequests.AddCustomer;
using Backend.Domain;
using Backend.Domain.Entities.User;
using Backend.Domain.Enum;
using Backend.Domain.Extensions;
using Backend.Infrastructure.Exceptions;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Business.Users.UsersRequests.CreateUser
{
    public class CreateUserRequestHandler : IRequestHandler<CreateUserRequest, ApplicationUser>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreateUserRequestHandler(
            IMediator mediator,
            IMapper mapper,
            IApplicationDbContext context)
        {
            _mediator = mediator;
            _mapper = mapper;
            _context = context;
        }

        public async Task<ApplicationUser> Handle(CreateUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                switch (request.AccountType)
                {
                    case AccountType.User:
                        return await CreateUser(request);

                    default:
                        throw new NotImplementedException($"This account type does not exist: {request.AccountType}");
                }
            }
            catch (Exception e)
            {
                throw new CreateFailureException(nameof(ApplicationUser), e);
            }
        }

        private async Task<ApplicationUser> CreateUser(CreateUserRequest request)
        {
            var user = _mapper.Map<CreateUserRequest, ApplicationUser>(request);

            // add to stripe
            user.CustomerId = await _mediator.Send(new AddCustomerRequest(user.GetFullName(), user.Email), CancellationToken.None);

            // TODO: Things tied to user creation

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            // send mail to complete registration
            await _mediator.Send(new SendRegistrationEmailRequest(user));

            return _mapper.Map<ApplicationUser>(user);
        }


    }
}