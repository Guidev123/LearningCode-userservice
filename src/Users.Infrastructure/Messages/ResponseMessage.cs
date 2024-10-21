using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Users.Application.Responses;

namespace Users.Infrastructure.Messages
{
    public class ResponseMessage<TData>
    {
        public Response<TData> Response { get; private set; } = null!;

        public ResponseMessage(Response<TData> response)
        {
            Response = response;
        }
    }
}
