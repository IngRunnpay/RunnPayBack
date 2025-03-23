using Dapper;
using Entities.Context.RunPayDb;
using Entities.General;
using Interfaces.DataAccess.Repository;
using Interfaces.DataAccess.Utilities;
using MethodsParameters.Input.Dispersion;
using MethodsParameters.Output.Dispersion;
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
    }
}
