using Entities.Context.RunPayDb;
using Entities.General;
using Interfaces.DataAccess.Repository;
using Interfaces.DataAccess.Utilities;
using MethodsParameters.Input;
using MethodsParameters.Input.Transaccion;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;
using System.Reflection.Metadata;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Amazon.SQS;
using Amazon;
using Amazon.CloudTrail.Model;
using MimeKit;
using MailKit.Net.Smtp;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace DataAccess.Repository
{
    public class NoticationRepository : INoticationRepository
    {
        private readonly IHelper _helper;
        private readonly IConfiguration _configuration;
        private readonly ILogRepository _logRepository;
        private readonly RunPayDbContext _RunPayDbContext;
        private readonly AmazonSimpleEmailServiceClient _amazonSesClient;
        private readonly string _senderEmail;
        public NoticationRepository(IHelper helper, IConfiguration configuration, RunPayDbContext runPayDbContext, ILogRepository logRepository)
        {
            _helper = helper;
            _configuration = configuration;
            _RunPayDbContext = runPayDbContext;
            _logRepository = logRepository;

            string accessKey = _configuration.GetSection("AWS:AccessKey").Value;
            string secretKey = _configuration.GetSection("AWS:SecretKey").Value;
            string region = _configuration.GetSection("AWS:Region").Value;
            _senderEmail = _configuration.GetSection("AWS:SenderEmail").Value;

            var awsRegion = RegionEndpoint.GetBySystemName(region);
            _amazonSesClient = new AmazonSimpleEmailServiceClient(accessKey, secretKey, awsRegion);
        }

        public async Task<BaseResponse> SendEmailOtp(NotificationOTp Request)
        {
            BaseResponse response = new BaseResponse();
            try
            {
                //var email = new MimeMessage();
                //email.From.Add(new MailboxAddress("See", "seecas515@gmail.com"));
                //email.To.Add(new MailboxAddress("Destinatario", Request.Destinatario));
                //email.Subject = "Código de verificación";
                //email.Body = new TextPart("plain") { Text = $"Tu código es {Request.Otp}" };

                //using (var smtp = new SmtpClient())
                //{
                //    smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                //    smtp.Authenticate("pruebasRunnPay", "sbqk risx fgde wtpc");
                //    smtp.Send(email);
                //    smtp.Disconnect(true);
                //}
                string subject = "Código de validación";
                string bodyText = $"Tu código de validación es: {Request.Otp}";

                var resEmail = await _amazonSesClient.SendEmailAsync(new SendEmailRequest
                {
                    Destination = new Destination
                    {
                        ToAddresses = new List<string> { Request.Destinatario }
                    },
                    Message = new Message
                    {
                        Body = new Body
                        {
                            Text = new Content
                            {
                                Charset = "UTF-8",
                                Data = bodyText
                            }
                        },
                        Subject = new Content
                        {
                            Charset = "UTF-8",
                            Data = subject
                        }
                    },
                    Source = _senderEmail
                });

                //response.CreateSuccess($"Correo enviado a {Request.Destinatario}. MessageId: {resEmail.MessageId}", resEmail.MessageId);


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
