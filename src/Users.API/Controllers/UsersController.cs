using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Users.Application.Command.CreateUser;
using Users.Application.Command.DeleteUser;
using Users.Application.Command.LoginUser;
using Users.Application.DTOs;
using Users.Application.Queries.GetUserById;
using Users.Application.Responses;
using Users.Domain.Entities;
using Users.Domain.Repositories;

namespace Users.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UsersController(IUnitOfWork uow, IMediator mediator) : ControllerBase
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly IMediator _mediator = mediator;

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<GetUserDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<GetUserDTO>))]
        [HttpGet("{id:guid}")]
        public async Task<IResult> GetByIdAsync(Guid id)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(id));

            if (result.IsSuccess)
                return TypedResults.Ok(result);

            return TypedResults.BadRequest(result);
        }

        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Response<GetUserDTO>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<GetUserDTO>))]
        [HttpPost("register")]
        public async Task<IResult> RegisterAsync(CreateUserCommand command)
        {
            var result = await _mediator.Send(command);

            if (await _uow.Commit() && result.IsSuccess)
                return TypedResults.Created($"api/users/{result.Data?.Id}", result);

            return TypedResults.BadRequest(result);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Response<string>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<string>))]
        [HttpPost("login")]
        public async Task<IResult> LoginAsync(LoginUserCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return TypedResults.Ok(result);

            return TypedResults.BadRequest(result);
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(Response<User>))]
        [HttpDelete("{id:guid}")]
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var result = await _mediator.Send(new DeleteUserCommand(id));

            if (await _uow.Commit() && result.IsSuccess)
                return TypedResults.NoContent();

            return TypedResults.BadRequest(result);
        }
    }
}
