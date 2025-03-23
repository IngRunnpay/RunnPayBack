using Dapper;
using Entities.Context.RunPayDb;
using Entities.General;
using Interfaces.DataAccess.Repository;
using Interfaces.DataAccess.Utilities;
using MethodsParameters.Input.Otp;
using MethodsParameters.Input.PortalUser;
using MethodsParameters.Output.PortalUser;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IHelper _helper;
        private readonly IConfiguration _configuration;
        private readonly RunPayDbContext _RunPayDbContext;

        public UserRepository(IHelper helper, IConfiguration configuration, RunPayDbContext runPayDbContext)
        {
            _helper = helper;
            _configuration = configuration;
            _RunPayDbContext = runPayDbContext;
        }
        public async Task<ResponseSpGetUserByEmail> GetUserByEmail(string Request)
        {
            BaseResponse response = new BaseResponse();
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Seguridad.Sp_GetInfoUsuarioXCorreo]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Correo", Request.ToLower());

            ResponseSpGetUserByEmail result = await _helper.ExecuteStoreProcedureFirstOrDefault<ResponseSpGetUserByEmail>(connectionString, storedProcedure, parameters);
           
            return result;
        }
    }
}
