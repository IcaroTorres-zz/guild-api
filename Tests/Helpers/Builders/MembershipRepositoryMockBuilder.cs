using Application.Common.Abstractions;
using Domain.Models;
using Moq;
using System;
using System.Linq.Expressions;
using Tests.Domain.Models.Fakes;

namespace Tests.Helpers.Builders
{
    public sealed class MembershipRepositoryMockBuilder
    {
        private readonly Mock<IMembershipRepository> _mock;

        private MembershipRepositoryMockBuilder()
        {
            _mock = new Mock<IMembershipRepository>();
        }

        public static MembershipRepositoryMockBuilder Create()
        {
            return new MembershipRepositoryMockBuilder();
        }

        public MembershipRepositoryMockBuilder Insert(Membership input = null, Membership output = null)
        {
            var result = output ?? MembershipFake.Active().Generate();

            if (input == null)
            {
                _mock.Setup(x => x.InsertAsync(It.IsAny<Membership>(), default)).ReturnsAsync(result);
            }
            else
            {
                _mock.Setup(x => x.InsertAsync(input, default)).ReturnsAsync(result);
            }

            return this;
        }

        public MembershipRepositoryMockBuilder Update(Membership input = null, Membership output = null)
        {
            var result = output ?? MembershipFake.Finished().Generate();

            if (input == null)
            {
                _mock.Setup(x => x.Update(It.IsAny<Membership>())).Returns(result);
            }
            else
            {
                _mock.Setup(x => x.Update(input)).Returns(result);
            }

            return this;
        }

        public MembershipRepositoryMockBuilder Paginate(int pageSize = 10, int page = 1, int totalItems = 20)
        {
            var PagedResponse = PagedResponseFake.PaginateMemberships(pageSize, page, totalItems);

            _mock.Setup(x => x.PaginateAsync(It.IsAny<Expression<Func<Membership, bool>>>(), pageSize, page, default)).ReturnsAsync(PagedResponse);

            return this;
        }

        public IMembershipRepository Build()
        {
            return _mock.Object;
        }
    }
}
