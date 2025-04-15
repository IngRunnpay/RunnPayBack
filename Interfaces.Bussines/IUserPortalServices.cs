using Entities.General;
using Entities.Input.PortalUser;
using Entities.Input.Transaccion;
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
        Task<BaseResponse> PerfilPortal(string IdAplicacion);
        Task<BaseResponse> PerfilUpdate(RequestPerfilUpdate request);



    }
}
