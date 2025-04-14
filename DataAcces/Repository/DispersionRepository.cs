using Dapper;
using Entities.Context.RunPayDb;
using Entities.General;
using Interfaces.DataAccess.Repository;
using Interfaces.DataAccess.Utilities;
using Entities.Input.Dispersion;
using Entities.Input.Reports;
using Entities.Output.Dispersion;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public  class DispersionRepository : IDispersionRepository
    {
        private readonly IHelper _helper;
        private readonly IConfiguration _configuration;
        private readonly RunPayDbContext _RunPayDbContext;

        public DispersionRepository(IHelper helper, IConfiguration configuration, RunPayDbContext runPayDbContext)
        {
            _helper = helper;
            _configuration = configuration;
            _RunPayDbContext = runPayDbContext;
        }
        public async Task<BaseResponse> PayOutSaldo(CreateDispersionSaldo request)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Dispersion.SpCreateDispersionSaldo]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdAplicacion", request.IdAplicacion);
            parameters.Add("@TipoDocumento", request.TipoDocumento);
            parameters.Add("@NumeroDocumento", request.NumeroDocumento);
            parameters.Add("@Banco", request.Banco);
            parameters.Add("@TipoCuenta", request.TipoCuenta);
            parameters.Add("@NumeroCuenta", request.NumeroCuenta);
            parameters.Add("@Monto", request.Monto);
            parameters.Add("@Referencia", request.Referencia);
            parameters.Add("@Descripcion", request.Descripcion);
            parameters.Add("@Telefono", request.Telefono);
            parameters.Add("@Correo", request.Correo);
            parameters.Add("@UsuNombre", request.UsuNombre);
            parameters.Add("@NotifyUrl", request.NotifyUrl);

            var result = await _helper.ExecuteStoreProcedureFirstOrDefault<int>(connectionString, storedProcedure, parameters);
            var response = new BaseResponse();
            response.CreateSuccess("Ok", result);

            return response;
        }
        public async Task<BaseResponse> TransaccionesDispersion(string request)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Dispersion.SP_GetTransaccionesDispersar]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdAplicacion", request);

            var result = await _helper.ExecuteStoreProcedureGet<object>(connectionString, storedProcedure, parameters);
            var response = new BaseResponse();
            response.CreateSuccess("Ok", result);

            return response;
        }
        public async Task<responseSpDataCuenta> DataCuenta(string request)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Dispersion.Sp_InfoCuentaPersonal]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdAplicacion", request);

            responseSpDataCuenta result = await _helper.ExecuteStoreProcedureFirstOrDefault<responseSpDataCuenta>(connectionString, storedProcedure, parameters);
          

            return result;
        }
        public async Task<object> ValidarTransaccionesLiquidadas(List<DataDispersion> request)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Dispersion.Sp_ValidarEstadoLiquidadas]";

            var dataTable = new DataTable();
            dataTable.Columns.Add("IdTransaccion", typeof(int));
            dataTable.Columns.Add("Referencia", typeof(string));

            foreach (var transaccion in request)
            {
                dataTable.Rows.Add(transaccion.IdTransaccion, transaccion.Referencia);
            }

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Transacciones", dataTable.AsTableValuedParameter("dbo.TransaccionesLista"));

            var result = await _helper.ExecuteStoreProcedureGetTable<object>(connectionString, storedProcedure, parameters);

            if (result.Count() <= 0 )
            {
                return null;
            }
            return result;
        }

        public async Task<int> CreateDispersion(CrearDispersion request)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Dispersion.Sp_CrearDispersionCuenta]";

            var dataTable = new DataTable();
            dataTable.Columns.Add("IdTransaccion", typeof(int));
            dataTable.Columns.Add("Referencia", typeof(string));

            foreach (var transaccion in request.Data)
            {
                dataTable.Rows.Add(transaccion.IdTransaccion, transaccion.Referencia);
            }

            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@Transacciones", dataTable.AsTableValuedParameter("dbo.TransaccionesLista"));
            parameters.Add("@IdAplicacion", request.IdAplicacion);
            parameters.Add("@TipoDispersion", request.TipoDispersion);
            parameters.Add("@Request", request.Request);


            var result = await _helper.ExecuteStoreProcedureFirstOrDefault<int>(connectionString, storedProcedure, parameters);

            return result;
        }
        public async Task<int> DesicionDispersion(RequestDecisionDispersion request)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[DispersionPayOutDesicion]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdDispersion", request.IdDispersion);
            parameters.Add("@IdEstado", request.IdEstado);


            var result = await _helper.ExecuteStoreProcedureFirstOrDefault<int>(connectionString, storedProcedure, parameters);
          

            return result;
        }
        public async Task<ResponseGetPyOut> GetDispersion(int IdDispersion)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Distribucion.Sp_GetInfoDitribucion]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdDistribucion", IdDispersion);

            ResponseGetPyOut result = await _helper.ExecuteStoreProcedureFirstOrDefault<ResponseGetPyOut>(connectionString, storedProcedure, parameters);


            return result;
        }

        public async Task<WebHookPayOut> GetWebHookDispersion(int IdDispersion)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Distribucion.Sp_WebHookPayOut]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdDispersion", IdDispersion);

            WebHookPayOut result = await _helper.ExecuteStoreProcedureFirstOrDefault<WebHookPayOut>(connectionString, storedProcedure, parameters);


            return result;
        }

        public async Task<ResponseGetPyOut> GetDispersionXReferencia(string referencia, string IdAplicacion)
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Distribucion.Sp_GetInfoDitribucionXReferencia]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdReferencia", referencia);
            parameters.Add("@IdAplicacion", IdAplicacion);


            ResponseGetPyOut result = await _helper.ExecuteStoreProcedureFirstOrDefault<ResponseGetPyOut>(connectionString, storedProcedure, parameters);


            return result;
        }

        public async Task<ReponseGetClientConfig> GetConfigClient(string IdAplicacion) 
        {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[Seguridad.Sp_DataConfigClient]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdAplicacion", IdAplicacion);


            ReponseGetClientConfig result = await _helper.ExecuteStoreProcedureFirstOrDefault<ReponseGetClientConfig>(connectionString, storedProcedure, parameters);


            return result;
        }

        public async Task<BaseResponse> Dispersion(RequestSpDispersion request) {
            string connectionString = _configuration.GetSection("ConnectionStrings:RunPayDbConnection").Value;
            string storedProcedure = "[dbo].[DispersionSaldo.Sp_Reporte]";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@IdAplicacion", request.IdAplicacion);
            parameters.Add("@Ini", request.Ini);
            parameters.Add("@Fin", request.Fin);
            parameters.Add("@IdDispersion", request.IdDispersion > 0 ? request.IdDispersion : null);
            parameters.Add("@Referencia", string.IsNullOrEmpty(request.Referencia) ? null : request.Referencia);
            parameters.Add("@Documento", string.IsNullOrEmpty(request.Documento) ? null : request.Documento);
            parameters.Add("@FechaInicio", string.IsNullOrEmpty(request.FechaInicio) ? null : request.FechaInicio);
            parameters.Add("@FechaFin", string.IsNullOrEmpty(request.FechaFin) ? null : request.FechaFin);

            var result = await _helper.ExecuteStoreProcedureGet<object>(connectionString, storedProcedure, parameters);
            var response = new BaseResponse();
            response.CreateSuccess("Ok", result);

            return response;
        }

    }
}
