using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.DTOs;
using Users.Application.Responses;

namespace Users.Application.Command.UpdateRoleUser
{
    public class UpdateRoleUserHandler : IRequestHandler<UpdateRoleUserCommand, Response<GetUserDTO>>
    {
        public Task<Response<GetUserDTO>> Handle(UpdateRoleUserCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
