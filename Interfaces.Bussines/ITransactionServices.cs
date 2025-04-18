using Entities.General;
using Entities.Input.Reports;
using Entities.Input.Transaccion;
using Entities.Output.Transaccion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.Bussines
{
    public interface ITransactionServices
    {
        Task<BaseResponse> Create(RequestTransactionCreate Request);
        Task<BaseResponse> ListTransaction(SpGetListTransactionByUser Request);
        Task<BaseResponse> HistoriTransaction(SpGetHistoriTransaction Request);
        Task<BaseResponse> Estadotransaccion(SpGetHistoriTransaction Request); 
        Task<BaseResponse> Resporttransaction(RequestSpTransactions request);
        Task<BaseResponse> Balance(string request);
        Task<BaseResponse> GatewayBank();
        Task<BaseResponse> PayInConsiliation(RequestPayInConsiliation request);
        Task<BaseResponse> PayInConsiliationExcel(RequestPayInConsiliationExcel request);
        Task<BaseResponse> DataComision(RequestDataComision request);

    }
}
