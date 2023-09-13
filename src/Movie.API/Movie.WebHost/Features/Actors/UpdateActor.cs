using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Movie.WebHost.Contracts;
using Movie.WebHost.Database;
using Movie.WebHost.Shared;

namespace Movie.WebHost.Features.Actors;

public class UpdateActor
{
    public class Command : IRequest<Result<Guid>>
    {
        public Guid Id { get; set; }
        
        public string FirstName { get; set; } = string.Empty;

        public string MiddleName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public int Age { get; set; }
    }

    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.FirstName).NotEmpty();
            RuleFor(x => x.MiddleName).NotEmpty();
            RuleFor(x => x.LastName).NotEmpty();
            RuleFor(x => x.Age).GreaterThan(0).LessThan(117);
        }
    }

    internal sealed class Handler : IRequestHandler<Command, Result<Guid>>
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IValidator<Command> _validator;

        public Handler(ApplicationDbContext dbContext, IValidator<Command> validator)
        {
            _dbContext = dbContext;
            _validator = validator;
        }

        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            var actor = await _dbContext
                .Actors
                .FindAsync(request.Id);

            if (actor is null)
            {
                return Result.Failure<Guid>(new Error(
                    "UpdateActor.Null",
                    $"Actor with id {request.Id} not found")
                );
            }

            actor.FirstName = request.FirstName;
            actor.MiddleName = request.MiddleName;
            actor.LastName = request.LastName;
            actor.Age = request.Age;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return actor.Id;
        }
    }
}

public class UpdateActorEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPatch("api/actors", async (UpdateActorRequest request, ISender sender) =>
        {
            var command = request.Adapt<UpdateActor.Command>();
            var result = await sender.Send(command);

            if (result.IsFailure)
                return Results.NotFound(result.Error);

            return Results.Ok(result.Value);
        });
    }
}