using Carter;
using FluentValidation;
using Mapster;
using MediatR;
using Movie.WebHost.Contracts;
using Movie.WebHost.Database;
using Movie.WebHost.Shared;

namespace Movie.WebHost.Features.Actors;

public static class CreateActor
{
    public class Command : IRequest<Result<Guid>>
    {
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
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return Result.Failure<Guid>(new Error(
                    "CreateActor.Validation",
                    validationResult.ToString()
                    )
                );
            }

            var actor = new Entities.Actor
            {
                Id = Guid.NewGuid(),
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                Age = request.Age,
            };

            await _dbContext.AddAsync(actor);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return actor.Id;
        }
    }
}

public class CreateActorEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("api/actors", async (CreateActorRequest request, ISender sender) =>
        {
            var command = request.Adapt<CreateActor.Command>();
            var result = await sender.Send(command);

            if (result.IsFailure)
                return Results.BadRequest(result.Error);

            return Results.Ok(result.Value);
        });
    }
}