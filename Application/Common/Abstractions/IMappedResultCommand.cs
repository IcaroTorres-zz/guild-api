using AutoMapper;

namespace Application.Common.Abstractions
{
    public interface IMappedResultCommand
    {
        object MapResult(object result, IMapper mapper);
    }
}