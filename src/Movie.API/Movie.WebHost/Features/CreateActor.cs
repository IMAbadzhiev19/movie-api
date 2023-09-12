using MediatR;
using Movie.WebHost.Shared;

namespace Movie.WebHost.Features;

public static class CreateActor
{
    public class Command : IRequest<Result<Guid>>
    {
        
    }
}
