using Carter;
using Mapster;
using MediatR;
using Movie.WebHost.Contracts;
using Movie.WebHost.Database;
using Movie.WebHost.Shared;

namespace Movie.WebHost.Features.Actors;

public class DeleteActor
{
    public class Command : IRequest<Result<bool>>
    {
        public Guid Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Command, Result<bool>>
    {
        private readonly ApplicationDbContext _dbContext;

        public Handler(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<Result<bool>> Handle(Command request, CancellationToken cancellationToken)
        {
            var actorToRemove = await this._dbContext.Actors.FindAsync(request.Id);

            if (actorToRemove is null)
            {
                return Result.Failure<bool>(
                    new Error(
                        "DeleteActor.Null",
                        $"Actor with id {request.Id} not found"
                    )
                );
            }

            this._dbContext.Actors.Remove(actorToRemove);
            await this._dbContext.SaveChangesAsync();

            return true;
        }
    }
}

public class DeleteActorEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("api/actors", async (DeleteActorRequest request, ISender sender) =>
        {
            var command = request.Adapt<DeleteActor.Command>();
            var result = await sender.Send(command);

            if (result.IsFailure)
                return Results.NotFound(result.Error);

            return Results.Ok(result.Value);
        });
    }
}