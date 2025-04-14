using Entities.General;
using Entities.Input.Otp;
using Entities.Input.PortalUser;
using Entities.Output.PortalUser;
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
