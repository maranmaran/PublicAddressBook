using Backend.Common;
using Backend.Common.Extensions;
using Backend.Domain;
using Backend.Domain.Entities.Contacts;
using Backend.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Business.Contacts.Requests.GetPaged
{
    public class GetPagedContactsRequestHandler : IRequestHandler<GetPagedContactsRequest, PagedList<Contact>>
    {
        private readonly IApplicationDbContext _context;

        public GetPagedContactsRequestHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<PagedList<Contact>> Handle(GetPagedContactsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var paginationModel = request.PaginationModel;

                // get all items
                var contacts = (IQueryable<Contact>)_context.Contacts.Include(x => x.PhoneNumbers);

                // filter
                if (!string.IsNullOrWhiteSpace(paginationModel.FilterQuery))
                {
                    var filterQueries = paginationModel.FilterQuery.ToLower().Split(" ");

                    foreach (var query in filterQueries)
                    {
                        contacts = contacts.Where(x =>
                            x.Name.ToLower().Contains(query) ||
                            x.Address.ToLower().Contains(query) ||
                            x.PhoneNumbers.Any(y => y.Number.ToLower().Contains(query)));
                    }
                }

                // get count of items
                var totalItems = contacts.Count();

                // sort
                if (!string.IsNullOrWhiteSpace(paginationModel.SortBy))
                {
                    contacts = (paginationModel.SortBy switch
                    {
                        "name" => contacts.Sort(contact => contact.Name, paginationModel.SortDirection),
                        "address" => contacts.Sort(type => type.Address, paginationModel.SortDirection),
                        _ => throw new Exception($"Can't filter by {paginationModel.SortBy}")
                    });
                }

                // page --- or fetch all results 
                var contactsPagedList = !request.PaginationModel.FetchAll ? contacts.Skip(paginationModel.Page * paginationModel.PageSize).Take(paginationModel.PageSize) : contacts;

                var list = await contactsPagedList.AsNoTracking().ToListAsync(cancellationToken);
                return new PagedList<Contact>(list, totalItems);
            }
            catch (Exception e)
            {
                throw new NotFoundException(nameof(Contact), $"Failed to get contacts for page {request.PaginationModel.Page}, pagesize: {request.PaginationModel.PageSize}", e);
            }
        }
    }
}
