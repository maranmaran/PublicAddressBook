using Backend.Common;
using Backend.Domain.Entities.Contacts;
using MediatR;

namespace Backend.Business.Contacts.Requests.GetPaged
{
    public class GetPagedContactsRequest : IRequest<PagedList<Contact>>
    {
        public PaginationModel PaginationModel { get; set; }
    }
}
