using Entities.General;
using MethodsParameters.Input;
using MethodsParameters.Input.Gateway;
using MethodsParameters.Input.Otp;
using MethodsParameters.Input.PortalUser;
using MethodsParameters.Output.Otp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DataAccess.Repository
{
    public interface IOtpRepository
    {
        Task<BaseResponse> OtpLogin(RequestOtpLogin Request);
        Task<ResponseSp_GetOtp> GetOtp(RequestValidOtp Request);
        Task<object> UpdateOtpValidado(UpdateOtpValidado Request);



    }
}
