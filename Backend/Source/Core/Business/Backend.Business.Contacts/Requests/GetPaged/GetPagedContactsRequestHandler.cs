using Backend.Common;
using Backend.Common.Extensions;
using Backend.Domain;
using Backend.Domain.Entities.Contacts;
using Backend.Infrastructure.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Backend.BusinessTests")]
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
                var contactsQuery = (IQueryable<Contact>)_context.Contacts.Include(x => x.PhoneNumbers);

                // filter
                contactsQuery = HandleFiltering(contactsQuery, paginationModel);

                // sort
                contactsQuery = HandleSort(contactsQuery, paginationModel);

                // get count of items
                var totalItems = contactsQuery.Count();

                // page <-> or fetch all results 
                contactsQuery = HandlePaging(contactsQuery, paginationModel);

                var contacts = await contactsQuery
                                        .AsNoTracking()
                                        .ToListAsync(cancellationToken);

                return new PagedList<Contact>(contacts, totalItems);
            }
            catch (Exception e)
            {
                throw new NotFoundException(nameof(Contact), $"Failed to get contacts for page {request.PaginationModel.Page}, pagesize: {request.PaginationModel.PageSize}", e);
            }
        }

        /// <summary>
        /// Appends paging to query or fetches all items based on configuration
        /// </summary>
        /// <param name="contacts"></param>
        /// <param name="paginationModel"></param>
        /// <returns></returns>
        internal IQueryable<Contact> HandlePaging(IQueryable<Contact> contacts, PaginationModel paginationModel)
        {
            if (!paginationModel.FetchAll)
            {
                return contacts
                    .Skip(paginationModel.Page * paginationModel.PageSize)
                    .Take(paginationModel.PageSize);
            }

            return contacts;
        }

        /// <summary>
        /// Appends sorting logic to query
        /// </summary>
        /// <param name="contacts"></param>
        /// <param name="paginationModel"></param>
        /// <returns></returns>
        internal IQueryable<Contact> HandleSort(IQueryable<Contact> contacts, PaginationModel paginationModel)
        {
            if (!string.IsNullOrWhiteSpace(paginationModel.SortBy))
            {
                contacts = (paginationModel.SortBy switch
                {
                    "name" => contacts.Sort(contact => contact.Name, paginationModel.SortDirection),
                    "address" => contacts.Sort(type => type.Address, paginationModel.SortDirection),
                    _ => throw new Exception($"Can't filter by {paginationModel.SortBy}")
                });
            }

            return contacts;
        }

        /// <summary>
        /// Appends filtering logic to query
        /// </summary>
        /// <param name="contacts"></param>
        /// <param name="paginationModel"></param>
        /// <returns></returns>
        internal IQueryable<Contact> HandleFiltering(IQueryable<Contact> contacts, PaginationModel paginationModel)
        {
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

            return contacts;
        }
    }
}
