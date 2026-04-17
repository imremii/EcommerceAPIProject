using Ecommerce.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecommerce.BLL
{
    public interface IAuthManager
    {
        Task<GeneralResult<AuthResponseDto>> RegisterAsync(RegisterDto dto);
        Task<GeneralResult<AuthResponseDto>> LoginAsync(LoginDto dto);
    }
}
