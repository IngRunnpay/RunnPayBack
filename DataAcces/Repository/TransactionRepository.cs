using Dapper;
using Entities.Context.RunPayDb;
using Entities.General;
using Entities.Identity;
using Interfaces.DataAccess.Repository;
using Interfaces.DataAccess.Utilities;
using Entities.Input.Reports;
using Entities.Input.Transaccion;
using Entities.Output.Dispersion;
using Entities.Output.Gateway;
using Entities.Output.Transaccion;
using Microsoft.Data.SqlClient.DataClassification;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DataAccess.Repository
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IHelper _helper;
        private readonly IConfiguration _configuration;
        private readonly RunPayDbContext _RunPayDbContext;

        public TransactionRepository(IHelper helper, IConfiguration configuration, RunPayDbContext runPayDbContext)
        {
            _helper = helper;
            _configuration = configuration;
            _RunPayDbContext = runPayDbContext;
        }
        public async Task<ResponseCreate> Create(RequestTransactionCreate Request)
        {
            ResponseCreate ObjResponse = new ResponseCreate();
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Transaccion.SpCreateTransaccion]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdMedioPago", Request.IdMedioPago);
            parameters.Add("@Monto", Request.Monto);
            parameters.Add("@IdTax", Request.IdTax);
            parameters.Add("@MontoFinal", Request.MontoFinal);
            parameters.Add("@Referencia", Request.Referencia);
            parameters.Add("@Descripcion", Request.Descripcion);
            parameters.Add("@FechaVencimiento", string.IsNullOrEmpty(Request.FechaVencimiento.ToString())? null : Request.FechaVencimiento);
            parameters.Add("@IdMoneda", Request.IdMoneda);
            parameters.Add("@TipoDocumento", Request.TipoDocumento);
            parameters.Add("@Documento", Request.Documento);
            parameters.Add("@Telefono", Request.Telefono);
            parameters.Add("@Correo", Request.Correo);
            parameters.Add("@Url", Request.Url);
            parameters.Add("@IdUsuario", Request.IdUsuario);
            parameters.Add("@UsuNombre", Request.UsuNombre);
            parameters.Add("@UrlCliente", Request.UrlCliente);



            ObjResponse.IdTransaccion = await _helper.ExecuteStoreProcedureFirstOrDefault<int>(connectionString, storedProcedure, parameters);          
            return ObjResponse;
        }

        public async Task<BaseResponse> UpdateCreate(RequestUpdateCreate Request)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Transaccion.Sp_UpdateCreateTransaction]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdTransaccion", Request.IdTransaccion);
            parameters.Add("@MontoFinal", Request.MontoFinal);
            parameters.Add("@Url", Request.Url);
            parameters.Add("@IdUsuario", Request.IdUsuario);
            var result = await _helper.ExecuteStoreProcedureFirstOrDefault<object>(connectionString, storedProcedure, parameters);
            var response = new BaseResponse();
            response.CreateSuccess("Ok", result);

            return response;
        }
        public async Task<BaseResponse> ListTransaction(SpGetListTransactionByUser Request)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Transaccion.Sp_GetListTransactionByUser]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdUsuario", Request.IdUsuario);

            var result = await _helper.ExecuteStoreProcedureGet<object>(connectionString, storedProcedure, parameters);
            var response = new BaseResponse();
            response.CreateSuccess("Ok", result);

            return response;
        }
        public async Task<BaseResponse> HistoriTransaction(SpGetHistoriTransaction Request)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Transaccion.Sp_GetTrazaTransaction]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdUsuario", Request.IdUsuario);
            parameters.Add("@IdTransaccion", Request.IdTransaccion);


            var result = await _helper.ExecuteStoreProcedureGet<object>(connectionString, storedProcedure, parameters);
            var response = new BaseResponse();
            response.CreateSuccess("Ok", result);

            return response;
        }
        public async Task<BaseResponse> UpdateTransaction(ActualizarEstadoTransaccion Request)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Transaccion.Sp_UpdateTransaction]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdTransaccion", Request.IdTransaccion);
            parameters.Add("@IdEstado", Request.idEstadoTransaccio);


            var result = await _helper.ExecuteStoreProcedureGet<object>(connectionString, storedProcedure, parameters);
            var response = new BaseResponse();
            response.CreateSuccess("Ok", result);

            return response;
        }
        public async Task<ResponseSp_GetDataTransaccion> DataTransaction(int IdTransaccion)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Transaccion.Sp_GetDataTransaccion]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdTransaccion", IdTransaccion);


            var result = await _helper.ExecuteStoreProcedureFirstOrDefault<ResponseSp_GetDataTransaccion>(connectionString, storedProcedure, parameters);
            return result;
        }

        public async Task<ResponseSpResumePay> QuicklypayController_ResumePay(int IdTransaccion)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Transaccion.Sp_GetResumePay]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdTransaccion", IdTransaccion);


            var result = await _helper.ExecuteStoreProcedureFirstOrDefault<ResponseSpResumePay>(connectionString, storedProcedure, parameters);
            return result;
        }
        public async Task<int> GetIdTranaccionbyReferenciaExterno(string token)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Transaccion.Sp_GetTransaccionByReferenciaExterna]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@ReferenciaExterna", token);

            var result = await _helper.ExecuteStoreProcedureFirstOrDefault<int>(connectionString, storedProcedure, parameters);

            return result;
        }
        public async Task<BaseResponse> GetBancosPSE()
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Informacion.GetBancosPSE]";
            DynamicParameters parameters = new DynamicParameters();

            var result = await _helper.ExecuteStoreProcedureGet<object>(connectionString, storedProcedure, parameters);

            var response = new BaseResponse();
            response.CreateSuccess("Ok", result);

            return response;
        }
        public async Task<ResponseSpDataWebHook> GetDataWebHook(int IdTransaccion)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Transaccion.Sp_GetStatusTransaccion]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdTransaccion", IdTransaccion);

            var result = await _helper.ExecuteStoreProcedureFirstOrDefault<ResponseSpDataWebHook>(connectionString, storedProcedure, parameters);

            return result;
        }
        public async Task<BaseResponse> Estadotransaccion(SpGetHistoriTransaction Request)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Transaccion.Sp_GetEstadOTransaccion]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdUsuario", Request.IdUsuario);
            parameters.Add("@IdTransaccion", Request.IdTransaccion);


            var result = await _helper.ExecuteStoreProcedureGet<object>(connectionString, storedProcedure, parameters);
            var response = new BaseResponse();
            response.CreateSuccess("Ok", result);

            return response;
        }
        public async Task<BaseResponse> Resporttransaction(RequestSpTransactions request)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Transaccion.Reporte]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdUsuario", request.IdUsuario);
            parameters.Add("@IdAplicacion", request.IdAplicacion);
            parameters.Add("@Ini", request.Ini);
            parameters.Add("@Fin", request.Fin);
            parameters.Add("@IdTransaccion", request.IdTransaccion > 0 ? request.IdTransaccion : null);
            parameters.Add("@Referencia", string.IsNullOrEmpty(request.Referencia) ? null : request.Referencia);
            parameters.Add("@Documento", string.IsNullOrEmpty(request.Documento) ? null : request.Documento);
            parameters.Add("@FechaInicio", string.IsNullOrEmpty(request.FechaInicio) ? null : request.FechaInicio);
            parameters.Add("@FechaFin", string.IsNullOrEmpty(request.FechaFin) ? null : request.FechaFin);
            parameters.Add("@IdEstado", request.IdEstado > 0 ? request.IdEstado : null);


            var result = await _helper.ExecuteStoreProcedureGet<object>(connectionString, storedProcedure, parameters);
            var response = new BaseResponse();
            response.CreateSuccess("Ok", result);

            return response;
        }
        public async Task<ReponseBalance> Balance(string request)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Liquidacion.Get_Balance]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdAplicacion", request);

            ReponseBalance result = await _helper.ExecuteStoreProcedureFirstOrDefault<ReponseBalance>(connectionString, storedProcedure, parameters);
           

            return result;
        }

        public async Task<BaseResponse> GetMetodPagoXUsuario(int IdTransaccion)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Transaccion.Sp_GetMetodoPago]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdTransaccion", IdTransaccion);

            var result = await _helper.ExecuteStoreProcedureGet<object>(connectionString, storedProcedure, parameters);
            var response = new BaseResponse();
            response.CreateSuccess("Ok", result);

            return response;
        }

        public async Task<BaseResponse> UpdateBePay(int IdTransaccion, int IdMedioPago)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Transaccion.Sp_UpdateBePay]";
            DynamicParameters parameters = new DynamicParameters(); 
            parameters.Add("@IdTransaccion", IdTransaccion);
            parameters.Add("@IdMedioPago", IdMedioPago);


            var result = await _helper.ExecuteStoreProcedureFirstOrDefault<object>(connectionString, storedProcedure, parameters);
            var response = new BaseResponse();
            response.CreateSuccess("Ok", result);

            return response;
        }
        public async Task<ReponseFullDataTransaction> FullDataTransaction(int IdTransaccion)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Transaccion.Sp_GetFullDataTransaccion]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdTransaccion", IdTransaccion);

            ReponseFullDataTransaction result = await _helper.ExecuteStoreProcedureFirstOrDefault<ReponseFullDataTransaction>(connectionString, storedProcedure, parameters);
     
            return result;
        }

        public async Task<BaseResponse> PayInConsiliation(RequestPayInConsiliation request)
        {
            BaseResponse response = new BaseResponse();
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Liquidacion.Sp_ConciliationPAYIN]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdAplicacion", request.IdAplicacion);
            parameters.Add("@Fecha", request.Fecha);
            parameters.Add("@Ini", request.Ini);
            parameters.Add("@Fin", request.Fin);

            var result = await _helper.ExecuteStoreProcedureGet<object>(connectionString, storedProcedure, parameters);
            response.CreateSuccess("OK", result);
            return response;
        }
        public async Task<BaseResponse> PayInConsiliationExcel(RequestPayInConsiliationExcel request)
        {
            BaseResponse response = new BaseResponse();
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Liquidacion.Sp_ExcelReportDayConciliationPAYIN]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdAplicacion", request.IdAplicacion);
            parameters.Add("@FechaIni", request.FechaIni);
            parameters.Add("@FechaFIn", request.FechaFin);

            var result = await _helper.ExecuteStoreProcedureGet<object>(connectionString, storedProcedure, parameters);
            response.CreateSuccess("OK", result);
            return response;
        }
        public async Task<BaseResponse> DataComision(RequestDataComision request)
        {
            BaseResponse response = new BaseResponse();
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Liquidacion.Sp_GetDayDataPayment]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdAplicacion", request.IdAplicacion);
            parameters.Add("@Fecha", request.Fecha);

            var result = await _helper.ExecuteStoreProcedureFirstOrDefault<object>(connectionString, storedProcedure, parameters);
            response.CreateSuccess("OK", result);
            return response;
        }
    }
}
