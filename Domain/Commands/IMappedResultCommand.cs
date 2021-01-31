using AutoMapper;

namespace Domain.Commands
{
    public interface IMappedResultCommand
    {
        object MapResult(object result, IMapper mapper);
    }
}