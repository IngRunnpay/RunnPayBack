using Dapper;
using Entities.Context.RunPayDb;
using Entities.General;
using Interfaces.DataAccess.Repository;
using Interfaces.DataAccess.Utilities;
using MethodsParameters.Input.Transaccion;
using MethodsParameters.Output.DashBoard;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class DashBoardRepository : IDashBoardRepository
    {
        private readonly IHelper _helper;
        private readonly IConfiguration _configuration;
        private readonly RunPayDbContext _RunPayDbContext;

        public DashBoardRepository(IHelper helper, IConfiguration configuration, RunPayDbContext runPayDbContext)
        {
            _helper = helper;
            _configuration = configuration;
            _RunPayDbContext = runPayDbContext;
        }
        public async Task<BaseResponse> DashboarHistorial(int IdUsuario)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Transaccion.Sp_GetDashboardHistorialTransaccion]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdUsuario", IdUsuario);

            var result = await _helper.ExecuteStoreProcedureGet<object>(connectionString, storedProcedure, parameters);
            var response = new BaseResponse();
            response.CreateSuccess("Ok", result);

            return response;
        }
        public async Task<ResponseDashboardMont> PorcentajeMensual(int IdUsuario, DateTime FechaInicio, DateTime FechaFin)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Transaccion.Sp_GetStatustransactionMont]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdUsuario", IdUsuario);
            parameters.Add("@FechaIncio", FechaInicio);
            parameters.Add("@FechaFin", FechaFin);

            ResponseDashboardMont result = await _helper.ExecuteStoreProcedureFirstOrDefault<ResponseDashboardMont>(connectionString, storedProcedure, parameters);

            return result;
        }
        public async Task<BaseResponse> TransaccionesAño(int IdUsuario)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Transaccion.Sp_GetTransaccionAnual]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdUsuario", IdUsuario);

            var result = await _helper.ExecuteStoreProcedureGet<object>(connectionString, storedProcedure, parameters);
            var response = new BaseResponse();
            response.CreateSuccess("Ok", result);

            return response;
        }
    }
}
