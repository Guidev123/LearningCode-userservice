using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Users.Application.DTOs
{
    public class GetUserTokensDTO
    {
        public GetUserTokensDTO(string token, string refreshToken)
        {
            Token = token;
            RefreshToken = refreshToken;
        }

        public string Token { get; private set; }
        public string RefreshToken { get; private set; }
    }
}
