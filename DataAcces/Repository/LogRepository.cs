using Dapper;
using Entities.General;
using Interfaces.DataAccess.Repository;
using Interfaces.DataAccess.Utilities;
using Entities.Input;
using Entities.Input.Gateway;
using Entities.Input.Transaccion;
using Entities.Output.Gateway;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class LogRepository : ILogRepository
    {
        private readonly IHelper _helper;
        private readonly IConfiguration _configuration;
        public LogRepository(IHelper helper, IConfiguration configuration)
        {
            _helper = helper;
            _configuration = configuration;
        }
        public async Task Logger(LogIn input)
        {
            Log.Error(string.Format("Error  Usuario : {0} ::: {1} ::: {2}", input.IdUsuarioAplicacion, input.Error, input.Detail));
            try
            {
                string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
                string query = "INSERT INTO [dbo].[Log.Aplicacion]([LogAplicacionFecha],[LogAplicacionFuenteError],[LogAplicacionMensaje],[LogAplicacionExcepcion],[LogAplicacionData1],[LogAplicacionData2])VALUES(getdate(),'{0}','{1}','{2}','{3}','{4}')";

                query = string.Format(query, input.Fuente, input.Detail, input.Error, input.Texto, input.IdUsuarioAplicacion);
                await _helper.ExecuteQuery(connectionString, query);
            }
            catch (Exception ex)
            {
                string code = DateTime.Now.Ticks.ToString();
                Log.Error(string.Format("Error _ {1} {0} ", input.Fuente, code));
                Log.Error(string.Format("Error _ {1} {0} ", input.Detail, code));
                Log.Error(string.Format("Error _ {1} {0} ", input.Error, code));
                Log.Error(string.Format("Error _ {1} {0} ", input.Texto, code));
                Log.Error(string.Format("Error _ {1} {0} ", ex.ToString(), code));
            }
        }
        public async Task<ResponseSpLogPasarelaExterna> LoggExternalPasarela(SpLogPasarelaExterna Request)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Log.Sp_CreateLogExternalPasarela]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdTransaccion", Request.IdTransaccion);
            parameters.Add("@Endpoint", Request.Endpoint);
            parameters.Add("@Request", Request.Request);
            parameters.Add("@Response", Request.Response);
            parameters.Add("@Enviada", Request.Enviada);


            var result = await _helper.ExecuteStoreProcedureFirstOrDefault<ResponseSpLogPasarelaExterna>(connectionString, storedProcedure, parameters);
            
            return result;
        }
        public async Task<bool> LogLinKPSEExternalPasarela(SpLogLinkPSEPasarelas Request)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Log.Sp_InsertLinkPsePasarelas]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdTransaccion", Request.IdTransaccion);
            parameters.Add("@Url", Request.Url);
            parameters.Add("@ReferenciaExterno", Request.ReferenciaExterno);



            var result = await _helper.ExecuteStoreProcedureFirstOrDefault<int>(connectionString, storedProcedure, parameters);

            return result > 0 ? true : false;
        }
        public async Task<string> GetLinkPSEExternalPasarela(int IdTransaccion)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Log.Sp_GetLinkPSEPasarelas]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdTransaccion", IdTransaccion);

            var result = await _helper.ExecuteStoreProcedureFirstOrDefault<string>(connectionString, storedProcedure, parameters);

            return result;
        }

        public async Task Loghttps(string tipo, string ruta, string request, string response)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[log.Sp_InsertConsumoHttps]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Tipo", tipo);
            parameters.Add("@Ruta", ruta);
            parameters.Add("@Request", request);
            parameters.Add("@Response", response);

            await _helper.ExecuteStoreProcedureFirstOrDefault<object>(connectionString, storedProcedure, parameters);

        }
    }
}
