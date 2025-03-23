using Entities.General;
using MethodsParameters.Input;
using MethodsParameters.Input.Otp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DataAccess.Repository
{
    public interface INoticationRepository
    {
        Task<BaseResponse> SendEmailOtp(NotificationOTp Request);
    }
}
