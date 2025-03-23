using Entities.General;
using MethodsParameters.Input.Otp;
using MethodsParameters.Input.PortalUser;
using MethodsParameters.Output.PortalUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DataAccess.Repository
{
    public interface IUserRepository
    {
        Task<ResponseSpGetUserByEmail> GetUserByEmail(string Request);

    }
}
