using Entities.General;
using Entities.Input;
using Entities.Input.Gateway;
using Entities.Input.Otp;
using Entities.Input.PortalUser;
using Entities.Output.Otp;
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
