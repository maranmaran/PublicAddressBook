using MediatR;

namespace Backend.Business.Billing.BillingRequests.AddCustomer
{

    public class AddCustomerRequest : IRequest<string>
    {
        public string FullName { get; set; }
        public string Email { get; set; }

        public AddCustomerRequest(string fullName, string email)
        {
            FullName = fullName;
            Email = email;
        }
    }
}
