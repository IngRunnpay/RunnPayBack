namespace ApiPublica.Routes
{
    public class RoutesPath
    {
        #region ControllerBase
        public const string ProducesJson = "application/json";
        public const string Base = "/Api";
        public const string BasePagination = "/{Pagina?}/{Total?}";
        public const string BaseOrder = "/{Columna?}/{Orden?}";
        #endregion

        #region AplicationValidateController
        public const string AplicationValidateController = "Aplicacion";
        public const string AplicationValidateController_LoginPKI = Base + "/" + AplicationValidateController + "/Login/";
        #endregion
        #region TransactionController
        public const string TransactionController = "Transaccion";
        public const string TransactionController_Create = Base + "/" + TransactionController + "/Create/";
        public const string TransactionController_ListTransaction = Base + "/" + TransactionController + "/ListTransaction/";
        public const string TransactionController_HistoriTransaction = Base + "/" + TransactionController + "/HistoriTransaction/";
        public const string TransactionController_Estadotransaccion = Base + "/" + TransactionController + "/Estadotransaccion/";
        public const string TransactionController_Balance = Base + "/" + TransactionController + "/Balance/";
        public const string TransactionController_Bancos = Base + "/" + TransactionController + "/Bancos/";

        #endregion
        #region DispersionController
        public const string DispersionController = "Dispersion";
        public const string DispersionController_PayOut = Base + "/" + DispersionController + "/PayOut/";
        //public const string DispersionController_TransaccionesDispersion = Base + "/" + DispersionController + "/TransaccionesDispersion/";
        //public const string DispersionController_DispersionCuenta= Base + "/" + DispersionController + "/DispersionCuenta/";
        //public const string DispersionController_DispersionTerceros = Base + "/" + DispersionController + "/DispersionTerceros/";

        #endregion
    }
}
