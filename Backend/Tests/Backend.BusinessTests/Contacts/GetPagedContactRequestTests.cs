using Backend.Business.Contacts.Requests.GetPaged;
using Backend.Common;
using Backend.Domain;
using Backend.Domain.Entities.Contacts;
using Backend.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Backend.BusinessTests.Contacts
{
    public class GetPagedContactsRequestTests
    {
        private readonly IApplicationDbContext _context;

        public GetPagedContactsRequestTests()
        {
            _context = TestHelper.GetContext();

            var contacts = new List<Contact>()
            {
                new Contact()
                {
                    Id = Guid.Parse("d5535764-d39d-42bf-9b84-429b72a6e902"),
                    Name = "Contact1",
                    Address = "Address1",
                    PhoneNumbers = new List<PhoneNumber>()
                    {
                        new PhoneNumber()
                        {
                            Number = "Number1",
                            Id = Guid.Parse("92d55817-9618-4fc3-ab93-7759307df904")
                        }
                    }
                },
                new Contact()
                {
                    Id = Guid.Parse("d5535764-d39d-42bf-9b84-429b72a6e903"),
                    Name = "Contact2",
                    Address = "Address2",
                    PhoneNumbers = new List<PhoneNumber>()
                    {
                        new PhoneNumber()
                        {
                            Number = "Number2",
                            Id = Guid.Parse("92d55817-9618-4fc3-ab93-7759307df905")
                        }
                    }
                },
            };

            _context.Contacts.AddRange(contacts);
            _context.SaveChangesAsync(CancellationToken.None).Wait();
        }

        #region Pagination

        [Fact]
        public async Task GetPagedContactsHandler_ValidRequest_GetsPaged()
        {

            var handler = new GetPagedContactsRequestHandler(_context);

            var request = new GetPagedContactsRequest()
            {
                PaginationModel = new PaginationModel()
                {
                    Page = 0,
                    PageSize = 1
                }
            };

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(1, result.List.Count());
            Assert.Equal(2, result.TotalItems);
        }

        [Fact]
        public async Task GetPagedContactsHandler_ValidRequest_GetsPagedAll()
        {

            var handler = new GetPagedContactsRequestHandler(_context);

            var request = new GetPagedContactsRequest()
            {
                PaginationModel = new PaginationModel()
                {
                    FetchAll = true
                }
            };

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(2, result.List.Count());
            Assert.Equal(2, result.TotalItems);
        }

        #endregion

        #region Sort


        [Fact]
        public async Task GetPagedContactsHandler_SortNameDesc_Sorts()
        {

            var handler = new GetPagedContactsRequestHandler(_context);

            var request = new GetPagedContactsRequest()
            {
                PaginationModel = new PaginationModel()
                {
                    SortBy = "name",
                    SortDirection = "desc",
                    Page = 0,
                    PageSize = 1
                }
            };

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal("Contact2", result.List.ElementAt(0).Name);
        }

        [Fact]
        public async Task GetPagedContactsHandler_SortAddressDesc_Sorts()
        {

            var handler = new GetPagedContactsRequestHandler(_context);

            var request = new GetPagedContactsRequest()
            {
                PaginationModel = new PaginationModel()
                {
                    SortBy = "address",
                    SortDirection = "desc",
                    Page = 0,
                    PageSize = 1
                }
            };

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal("Address2", result.List.ElementAt(0).Address);
        }

        [Fact]
        public async Task GetPagedContactsHandler_SortNameAsc_Sorts()
        {

            var handler = new GetPagedContactsRequestHandler(_context);

            var request = new GetPagedContactsRequest()
            {
                PaginationModel = new PaginationModel()
                {
                    SortBy = "name",
                    SortDirection = "asc",
                    Page = 0,
                    PageSize = 1
                }
            };

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal("Contact1", result.List.ElementAt(0).Name);
        }

        [Fact]
        public async Task GetPagedContactsHandler_SortAddressAsc_Sorts()
        {

            var handler = new GetPagedContactsRequestHandler(_context);

            var request = new GetPagedContactsRequest()
            {
                PaginationModel = new PaginationModel()
                {
                    SortBy = "address",
                    SortDirection = "asc",
                    Page = 0,
                    PageSize = 1
                }
            };

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal("Address1", result.List.ElementAt(0).Address);
        }

        [Fact]
        public async Task GetPagedContactsHandler_SortNotFound_Throws()
        {

            var handler = new GetPagedContactsRequestHandler(_context);

            var request = new GetPagedContactsRequest()
            {
                PaginationModel = new PaginationModel()
                {
                    SortBy = "something",
                    SortDirection = "asc",
                    Page = 0,
                    PageSize = 1
                }
            };

            await Assert.ThrowsAsync<NotFoundException>(() => handler.Handle(request, CancellationToken.None));
        }

        #endregion

        #region Filter

        [Fact]
        public async Task GetPagedContactsHandler_FilterName_Filters()
        {

            var handler = new GetPagedContactsRequestHandler(_context);

            var request = new GetPagedContactsRequest()
            {
                PaginationModel = new PaginationModel()
                {
                    FilterQuery = "Contact1",
                    Page = 0,
                    PageSize = 1
                }
            };

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal("Contact1", result.List.ElementAt(0).Name);
        }

        [Fact]
        public async Task GetPagedContactsHandler_FilterNumber_Filters()
        {

            var handler = new GetPagedContactsRequestHandler(_context);

            var request = new GetPagedContactsRequest()
            {
                PaginationModel = new PaginationModel()
                {
                    FilterQuery = "Number2",
                    Page = 0,
                    PageSize = 1
                }
            };

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal("Number2", result.List.ElementAt(0).PhoneNumbers.First().Number);
        }

        [Fact]
        public async Task GetPagedContactsHandler_FilterAddress_Filters()
        {

            var handler = new GetPagedContactsRequestHandler(_context);

            var request = new GetPagedContactsRequest()
            {
                PaginationModel = new PaginationModel()
                {
                    FilterQuery = "Address1",
                    Page = 0,
                    PageSize = 1
                }
            };

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal("Address1", result.List.ElementAt(0).Address);
        }

        [Fact]
        public async Task GetPagedContactsHandler_FilterNameAddress_Filters()
        {

            var handler = new GetPagedContactsRequestHandler(_context);

            var request = new GetPagedContactsRequest()
            {
                PaginationModel = new PaginationModel()
                {
                    FilterQuery = "Contact1 Address1",
                    Page = 0,
                    PageSize = 1
                }
            };

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal("Contact1", result.List.ElementAt(0).Name);
            Assert.Equal("Address1", result.List.ElementAt(0).Address);
        }

        [Fact]
        public async Task GetPagedContactsHandler_FilterUnknown_FindsZero()
        {

            var handler = new GetPagedContactsRequestHandler(_context);

            var request = new GetPagedContactsRequest()
            {
                PaginationModel = new PaginationModel()
                {
                    FilterQuery = "Something",
                    Page = 0,
                    PageSize = 1
                }
            };

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(0, result.List.Count());
        }

        #endregion
    }
}