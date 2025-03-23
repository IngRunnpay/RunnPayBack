using Entities.General;
using MethodsParameters.Input.PortalUser;
using MethodsParameters.Input.Transaccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Bussines
{
    public interface IUserPortalServices
    {
        Task<BaseResponse> LoginPortal(Login request);
        Task<BaseResponse> ValidOtp(RequestValidOtp request);

    }
}
