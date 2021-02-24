using Application.Common.Abstractions;
using Domain.Enums;
using Domain.Models;
using Moq;
using System;
using System.Linq.Expressions;
using Tests.Domain.Models.Fakes;

namespace Tests.Helpers.Builders
{
    public sealed class InviteRepositoryMockBuilder
    {
        private readonly Mock<IInviteRepository> _mock;

        private InviteRepositoryMockBuilder()
        {
            _mock = new Mock<IInviteRepository>();
        }

        public static InviteRepositoryMockBuilder Create()
        {
            return new InviteRepositoryMockBuilder();
        }

        public InviteRepositoryMockBuilder GetByIdSuccess(Guid? input = null, Invite output = null)
        {
            var result = output ?? InviteFake.ValidWithStatus().Generate();

            if (input == null)
            {
                _mock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), default)).ReturnsAsync(result);
            }
            else
            {
                _mock.Setup(x => x.GetByIdAsync(output.Id, It.IsAny<bool>(), default)).ReturnsAsync(result);
            }

            return this;
        }

        public InviteRepositoryMockBuilder GetByIdFail(Guid? id = null)
        {
            var result = Invite.Null;

            if (id == null)
            {
                _mock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), default)).ReturnsAsync(result);
            }
            else
            {
                _mock.Setup(x => x.GetByIdAsync(id.Value, It.IsAny<bool>(), default)).ReturnsAsync(result);
            }

            return this;
        }

        public InviteRepositoryMockBuilder GetForAcceptOperationSuccess(Guid? input = null, Invite output = null)
        {
            var result = output ?? InviteFake.ValidWithStatus().Generate();

            if (input == null)
            {
                _mock.Setup(x => x.GetForAcceptOperationAsync(It.IsAny<Guid>(), default)).ReturnsAsync(result);
            }
            else
            {
                _mock.Setup(x => x.GetForAcceptOperationAsync(output.Id, default)).ReturnsAsync(result);
            }

            return this;
        }

        public InviteRepositoryMockBuilder GetForAcceptOperationFail(Guid? id = null)
        {
            var result = Invite.Null;

            if (id == null)
            {
                _mock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<bool>(), default)).ReturnsAsync(result);
            }
            else
            {
                _mock.Setup(x => x.GetByIdAsync(id.Value, It.IsAny<bool>(), default)).ReturnsAsync(result);
            }

            return this;
        }

        public InviteRepositoryMockBuilder Insert(Invite input = null, Invite output = null)
        {
            var result = output ?? InviteFake.ValidWithStatus(InviteStatuses.Pending).Generate();

            if (input == null)
            {
                _mock.Setup(x => x.InsertAsync(It.IsAny<Invite>(), default)).ReturnsAsync(result);
            }
            else
            {
                _mock.Setup(x => x.InsertAsync(input, default)).ReturnsAsync(result);
            }

            return this;
        }
        public InviteRepositoryMockBuilder Update(Invite input = null, Invite output = null)
        {
            var result = output ?? InviteFake.ValidWithStatus(InviteStatuses.Accepted).Generate();

            if (input == null)
            {
                _mock.Setup(x => x.Update(It.IsAny<Invite>())).Returns(result);
            }
            else
            {
                _mock.Setup(x => x.Update(input)).Returns(result);
            }

            return this;
        }

        public InviteRepositoryMockBuilder Paginate(int pageSize = 10, int page = 1, int totalItems = 20)
        {
            var PagedResponse = PagedResponseFake.PaginateInvites(pageSize, page, totalItems);

            _mock.Setup(x => x.PaginateAsync(It.IsAny<Expression<Func<Invite, bool>>>(), pageSize, page, default))
                 .ReturnsAsync(PagedResponse);

            return this;
        }

        public IInviteRepository Build()
        {
            return _mock.Object;
        }
    }
}
