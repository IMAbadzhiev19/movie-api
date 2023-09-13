using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie.WebHost.Contracts;
using Movie.WebHost.Database;
using Movie.WebHost.Shared;

namespace Movie.WebHost.Features.Actors;

public static class GetActor
{
    public class Query : IRequest<Result<ActorResponse>>
    {
        public Guid Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, Result<ActorResponse>>
    {
        private readonly ApplicationDbContext _dbContext;

        public Handler(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        

        public async Task<Result<ActorResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var actorResponse = await this._dbContext
                .Actors
                .AsNoTracking()
                .Where(a => a.Id == request.Id)
                .Select(a => new ActorResponse
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    MiddleName = a.MiddleName,
                    LastName = a.LastName,
                    Age = a.Age,
                    Movies = a.Movies
                })
                .FirstOrDefaultAsync();

            if (actorResponse is null)
            {
                return Result.Failure<ActorResponse>(new Error(
                        "GetActor.Null",
                        $"The actor with id {request.Id} was not found"
                    )
                );
            }

            return actorResponse;
        }
    }
}

public class GetActorEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/actors/{id}", async ([FromRoute] Guid id, ISender sender) =>
        {
            var query = new GetActor.Query { Id = id };
            var result = await sender.Send(query);

            if (result.IsFailure)
                return Results.NotFound(result.Error);

            return Results.Ok(result.Value);
        });
    }
}