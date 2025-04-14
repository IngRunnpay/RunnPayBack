using Entities.General;
using Entities.Input.Reports;
using Entities.Input.Transaccion;
using Entities.Output.Dispersion;
using Entities.Output.Gateway;
using Entities.Output.Transaccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.DataAccess.Repository
{
    public interface ITransactionRepository
    {
        Task<ResponseCreate> Create(RequestTransactionCreate Request);
        Task<BaseResponse> UpdateCreate(RequestUpdateCreate Request);
        Task<BaseResponse> ListTransaction(SpGetListTransactionByUser Request);
        Task<BaseResponse> HistoriTransaction(SpGetHistoriTransaction Request);
        Task<BaseResponse> UpdateTransaction(ActualizarEstadoTransaccion Request); 
        Task<ResponseSp_GetDataTransaccion> DataTransaction(int IdTransaccion);
        Task<ResponseSpResumePay> QuicklypayController_ResumePay(int IdTransaccion);
        Task<int> GetIdTranaccionbyReferenciaExterno(string token);
        Task<BaseResponse> GetBancosPSE();
        Task<ResponseSpDataWebHook> GetDataWebHook(int IdTransaccion);
        Task<BaseResponse> Estadotransaccion(SpGetHistoriTransaction Request); 
        Task<BaseResponse> Resporttransaction(RequestSpTransactions request); 
        Task<ReponseBalance> Balance(string request);
        Task<BaseResponse> GetMetodPagoXUsuario(int IdTransaccion);
        Task<BaseResponse> UpdateBePay(int IdTransaccion, int IdMedioPago);
        Task<ReponseFullDataTransaction> FullDataTransaction(int IdTransaccion);


    }
}
