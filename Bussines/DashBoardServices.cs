using Entities.General;
using Interfaces.Bussines;
using Interfaces.DataAccess.Repository;
using MethodsParameters.Input;
using MethodsParameters.Output.Transaccion;
using MethodsParameters;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MethodsParameters.Utilities;
using System.Web;
using MethodsParameters.Output.DashBoard;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Bussines
{
    public class DashBoardServices : IDashBoardServices
    {
        private readonly IConfiguration _configuration;
        private readonly ILogRepository _logRepository;
        public readonly IDashBoardRepository _dashboardRepository;
        public DashBoardServices(IConfiguration configuration, ILogRepository logRepository, IDashBoardRepository dashboardRepository)
        {
            _configuration = configuration;
            _logRepository = logRepository;
            _dashboardRepository = dashboardRepository;
        }
        public async Task<BaseResponse> DashboarHistorial(int IdUsuario)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var resp = await _dashboardRepository.DashboarHistorial(IdUsuario);
                response.CreateSuccess("OK", resp.Data);
            }
            catch (CustomException ex)
            {
                response.CreateError(ex);
            }
            catch (Exception ex)
            {
                await _logRepository.Logger(new LogIn(ex));
                response.CreateError(ex);
            }
            return response;
        }
        public async Task<BaseResponse> PorcentajeMensual(int IdUsuario)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                DateTime fechaActual = DateTime.Now;
                DateTime primerDiaMes = new DateTime(fechaActual.Year, fechaActual.Month, 1, 0, 0, 0, 0);
                DateTime ultimoDiaMes = new DateTime(fechaActual.Year, fechaActual.Month,
                                                     DateTime.DaysInMonth(fechaActual.Year, fechaActual.Month),
                                                     23, 59, 59, 000);
                ResponseDashboardMont resp = await _dashboardRepository.PorcentajeMensual(IdUsuario, primerDiaMes, ultimoDiaMes);

                object Porcentaje = new
                {
                    porcentajeAprobado = Math.Round(((resp.MontoAprobado * 100) / resp.SumaTotal), 2),
                    montoAprobado = resp.MontoAprobado,
                    porcentajePendiente = Math.Round(((resp.MontoPendiente * 100) / resp.SumaTotal), 2),
                    montoPendiente = resp.MontoPendiente,
                    porcentajeRechazo = Math.Round((resp.MontoRechazado * 100) / resp.SumaTotal, 2),
                    montoRechazo = resp.MontoRechazado

                };
                response.CreateSuccess("OK", Porcentaje);
            }
            catch (CustomException ex)
            {
                response.CreateError(ex);
            }
            catch (Exception ex)
            {
                await _logRepository.Logger(new LogIn(ex));
                response.CreateError(ex);
            }
            return response;
        }

        public async Task<BaseResponse> TransaccionesAño(int IdUsuario)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                var resp = await _dashboardRepository.TransaccionesAño(IdUsuario);
                response.CreateSuccess("OK", resp.Data);
            }
            catch (CustomException ex)
            {
                response.CreateError(ex);
            }
            catch (Exception ex)
            {
                await _logRepository.Logger(new LogIn(ex));
                response.CreateError(ex);
            }
            return response;
        }
    }
}
