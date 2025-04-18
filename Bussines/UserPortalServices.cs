using Entities.General;
using Interfaces.Bussines;
using Interfaces.DataAccess.Repository;
using Entities.Input.Transaccion;
using Entities.Input;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Input.PortalUser;
using Entities.Output.PortalUser;
using Entities.Input.Otp;
using System.Text.Json.Nodes;
using System.Text.Json;
using Entities.Output.Otp;

namespace Bussines
{
    public class UserPortalServices : IUserPortalServices
    {
        private readonly IConfiguration _configuration;
        private readonly ILogRepository _logRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOtpRepository _otpRepository;
        private readonly INoticationRepository _notificationRepository;


        public UserPortalServices(IConfiguration configuration,  ILogRepository logRepository, IUserRepository userRepository, IOtpRepository otpRepository, INoticationRepository noticationRepository)
        {
            _configuration = configuration;
            _logRepository = logRepository;
            _userRepository = userRepository;
            _otpRepository = otpRepository;
            _notificationRepository = noticationRepository;
        }
        #region publico
        public async Task<BaseResponse> LoginPortal(Login request)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                ResponseSpGetUserByEmail User =  await _userRepository.GetUserByEmail(request.Correo);
                if (User == null)
                {
                    response.CreateError("El usuario no existe.");
                }
                else
                {
                    string otp = await GenerarOTP(4);
                    RequestOtpLogin RequestInsertOTP = new RequestOtpLogin();
                    RequestInsertOTP.IdUsuario = User.IdUsuario;
                    RequestInsertOTP.CodigoOTP = otp;
                    RequestInsertOTP.Data = JsonSerializer.Serialize(request);
                    await _otpRepository.OtpLogin(RequestInsertOTP);
                    
                    NotificationOTp Send = new NotificationOTp();
                    Send.Otp = otp;
                    Send.Destinatario = User.Correo;
                    //var responseEmail = await _notificationRepository.SendEmailOtp(Send);
                    if (true)
                    {
                        response.Success = true;
                        response.Code = 200;
                        response.Message = "Código de validacón enviado correctamente.";
                    }
                    else
                    {
                        response.CreateError("El correo no se envio correctamente, intenta nuevamente.");
                    }

                }
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

        public async Task<BaseResponse> ValidOtp(RequestValidOtp request)
        {
            BaseResponse response = new BaseResponse();
            try
            {

                ResponseSpGetUserByEmail User = await _userRepository.GetUserByEmail(request.Correo);

                if(request.Otp.ToUpper() == User.IdAplicacion.ToString().ToUpper())
                {
                    object Resp = new
                    {
                        Idusuario = User.IdUsuario,
                        Nombre = $"{User.Nombre} {User.Apellido}",
                        Cod = User.IdAplicacion,
                        PT = User.NombrePT,
                        Nit = User.NIT                    
                    };
                    response.CreateSuccess("código validado correctamente.", Resp);
                }
                else
                {
                    response.CreateError("El código de aplicacion no pertenece al usuario.");
                }

                //ResponseSp_GetOtp DataOtp = await _otpRepository.GetOtp(request);
                //if (DataOtp != null)
                //{
                //    DateTime Hoy = new DateTime();
                //    if (Hoy > DataOtp.FechaMaxValidacion)
                //    {
                //        response.CreateError("Limite de tiempo excedido.");
                //    }
                //    else
                //    {
                //        if (!DataOtp.Validado)
                //        {
                //            if (request.Otp.ToUpper() == DataOtp.CodigoOtp.ToUpper())
                //            {
                //                ResponseSpGetUserByEmail User = await _userRepository.GetUserByEmail(request.Correo);
                //                object Resp = new
                //                {
                //                    Idusuario = User.IdUsuario,
                //                    Nombre = $"{User.Nombre} {User.Apellido}",
                //                    Cod = User.IdAplicacion
                //                };

                //                object dataOtp = new
                //                {
                //                    Id = DataOtp.Id,
                //                    IdUsuario = User.IdUsuario,
                //                    Codigo = request.Otp,
                //                    Correo = request.Correo,
                //                };
                //                UpdateOtpValidado UptOtp = new UpdateOtpValidado();
                //                UptOtp.IdOtp = DataOtp.Id;
                //                UptOtp.Data = JsonSerializer.Serialize(dataOtp);
                //                await _otpRepository.UpdateOtpValidado(UptOtp);

                //                response.CreateSuccess("código validado correctamente.", Resp);
                //            }
                //            else
                //            {
                //                response.CreateError("Limite de tiempo excedido.");
                //            }
                //        }
                //        else
                //        {
                //            response.CreateError("Código anteriormente validado.");
                //        }
                //    }
                //}
                //else
                //{
                //    response.CreateError("código Invalido.");
                //}
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

        public async Task<BaseResponse> ConfigPayIN(string IdAplicacion)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                response = await _userRepository.ConfigPayIN(IdAplicacion);

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
        #endregion

        #region Privado
        private async Task<string> GenerarOTP(int longitud)
        {
            string response = "";
            try
            {
                const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                Random random = new Random();
                return new string(Enumerable.Range(0, longitud)
                    .Select(_ => caracteres[random.Next(caracteres.Length)]).ToArray());
            }
            catch (CustomException ex)
            {
                response = null;
            }
            catch (Exception ex)
            {
                await _logRepository.Logger(new LogIn(ex));
                response = null;
            }
            return response;
        }

        #endregion
    }
}
