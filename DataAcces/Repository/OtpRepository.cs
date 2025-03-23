using Dapper;
using Entities.Context.RunPayDb;
using Entities.General;
using Interfaces.DataAccess.Repository;
using Interfaces.DataAccess.Utilities;
using MethodsParameters.Input.Otp;
using MethodsParameters.Input.PortalUser;
using MethodsParameters.Output.DashBoard;
using MethodsParameters.Output.Otp;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class OtpRepository : IOtpRepository
    {
        private readonly IHelper _helper;
        private readonly IConfiguration _configuration;
        private readonly RunPayDbContext _RunPayDbContext;

        public OtpRepository(IHelper helper, IConfiguration configuration, RunPayDbContext runPayDbContext)
        {
            _helper = helper;
            _configuration = configuration;
            _RunPayDbContext = runPayDbContext;
        }
        public async Task<BaseResponse> OtpLogin(RequestOtpLogin Request)
        {
            BaseResponse response = new BaseResponse();
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Otp.Sp_InsertOtpLogin]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdUsuario", Request.IdUsuario);
            parameters.Add("@CodigoOTP", Request.CodigoOTP);
            parameters.Add("@Data", Request.Data);

            int result = await _helper.ExecuteStoreProcedureFirstOrDefault<int>(connectionString, storedProcedure, parameters);
            response.CreateSuccess("Ok", result);

            return response;
        }

        public async Task<ResponseSp_GetOtp> GetOtp(RequestValidOtp Request)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Otp.Sp_GetOtpLogin]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Correo", Request.Correo.ToLower());
            parameters.Add("@Otp", Request.Otp);

            ResponseSp_GetOtp result = await _helper.ExecuteStoreProcedureFirstOrDefault<ResponseSp_GetOtp>(connectionString, storedProcedure, parameters);

            return result;
        }
        public async Task<object> UpdateOtpValidado(UpdateOtpValidado Request)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Otp.Sp_UpdateOtpValidado]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdOtp", Request.IdOtp);
            parameters.Add("@Data", Request.Data);

            object result = await _helper.ExecuteStoreProcedureFirstOrDefault<object>(connectionString, storedProcedure, parameters);

            return result;
        }
        
    }
}
